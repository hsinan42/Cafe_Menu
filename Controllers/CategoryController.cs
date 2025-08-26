using BusinessLayer.Abstract;
using BusinessLayer.Concrete;
using BusinessLayer.ValidationRules;
using DataAccessLayer.EntityFramework;
using EntityLayer.Concrete;
using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Cafe_Menu.Controllers
{
    public class CategoryController : Controller
    {
        CategoryManager cm = new CategoryManager(new EfCategoryDal());
        public ActionResult Index()
        {
            var categoryvalues = cm.GetList();

            return View(categoryvalues);
        }
        [Authorize]
        [HttpGet]
        public ActionResult CategoryAdd()
        {
            return View();
        }
        [Authorize]
        [HttpPost]
        public ActionResult CategoryAdd(Category p, HttpPostedFileBase CategoryImage)
        {
            // Validasyon
            CategoryValidator categoryValidator = new CategoryValidator();
            ValidationResult results = categoryValidator.Validate(p);
            p.CategoryStatus = true;

            if (results.IsValid)
            {
                if (CategoryImage != null && CategoryImage.ContentLength > 0)
                {
                    try
                    {
                        var existingCategory = cm.GetList().FirstOrDefault(x => x.CategoryID == p.CategoryID);
                        if (existingCategory != null && !string.IsNullOrEmpty(existingCategory.CategoryImage))
                        {
                            string oldImagePath = Server.MapPath(existingCategory.CategoryImage);
                            if (System.IO.File.Exists(oldImagePath))
                            {
                                // Dosya hash kontrolü yap
                                string oldHash = FileHelper.ComputeFileHash(oldImagePath);
                                string newHash = FileHelper.ComputeFileHash(CategoryImage);

                                if (oldHash != newHash) // Eğer aynı değilse sil
                                {
                                    System.IO.File.Delete(oldImagePath);
                                }
                            }
                        }

                        // Aynı ismi kullanarak kaydet (dosya ismi değişmesin)
                        string filePath = Path.Combine(Server.MapPath("~/Uploads/"), CategoryImage.FileName);
                        CategoryImage.SaveAs(filePath);

                        // Yeni resmin yolunu veritabanına kaydet
                        p.CategoryImage = "/Uploads/" + CategoryImage.FileName;
                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError("", "Resim yüklenirken hata oluştu: " + ex.Message);
                    }
                }

                // Veritabanına kategori ekle
                cm.CategoryAdd(p);
                return RedirectToAction("CategoryAdd");
            }
            else
            {
                foreach (var item in results.Errors)
                {
                    ModelState.AddModelError(item.PropertyName, item.ErrorMessage);
                }
            }

            return View();
        }
        [Authorize]
        [HttpGet]
        public ActionResult Categories()
        {
            var categories = cm.GetList();
            return View(categories);
        }
        [Authorize]
        [HttpGet]
        public ActionResult CategoryEdit(int id)
        {
            var categoryvalues = cm.GetCategoryByID(id);
            return View(categoryvalues);
        }
        [Authorize]
        [HttpPost]
        public ActionResult CategoryEdit(int id, Category p, HttpPostedFileBase CategoryImage)
        {
            // Fetch the existing category from the database
            var existingCategory = cm.GetCategoryByID(id);

            if (existingCategory == null)
            {
                return HttpNotFound();
            }

            // Validate input
            CategoryValidator categoryValidator = new CategoryValidator();
            ValidationResult results = categoryValidator.Validate(p);

            if (results.IsValid)
            {
                // Update properties except the primary key
                existingCategory.CategoryName = p.CategoryName;
                existingCategory.CategoryDescription = p.CategoryDescription;

                // Handle Image Upload
                if (CategoryImage != null && CategoryImage.ContentLength > 0)
                {
                    try
                    {
                        // Delete the old image if a new one is uploaded
                        if (!string.IsNullOrEmpty(existingCategory.CategoryImage))
                        {
                            string oldImagePath = Server.MapPath(existingCategory.CategoryImage);
                            if (System.IO.File.Exists(oldImagePath))
                            {
                                System.IO.File.Delete(oldImagePath);
                            }
                        }

                        // Save the new image
                        string filePath = Path.Combine(Server.MapPath("~/Uploads/"), CategoryImage.FileName);
                        CategoryImage.SaveAs(filePath);
                        existingCategory.CategoryImage = "/Uploads/" + CategoryImage.FileName;
                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError("", "Resim yüklenirken hata oluştu: " + ex.Message);
                    }
                }

                // Update the category
                cm.CategoryUpdate(existingCategory);

                return RedirectToAction("Categories");
            }
            else
            {
                foreach (var item in results.Errors)
                {
                    ModelState.AddModelError(item.PropertyName, item.ErrorMessage);
                }
            }

            return View(existingCategory);
        }
        [Authorize]
        [HttpPost]
        public JsonResult ToggleStatus(int id)
        {
            var category = cm.GetCategoryByID(id);
            if (category == null)
            {
                return Json(new { success = false });
            }

            category.CategoryStatus = !category.CategoryStatus;

            cm.CategoryUpdate(category);

            return Json(new { success = true, newStatus = category.CategoryStatus });
        }
        [Authorize]
        [HttpPost]
        public JsonResult CategoryDelete(int id)
        {
            var category = cm.GetCategoryByID(id);
            if (category == null)
            {
                return Json(new { success = false });
            }

            cm.CategoryDelete(category);

            return Json(new { success = true });
        }
    }
}