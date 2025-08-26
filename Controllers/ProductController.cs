using BusinessLayer.Concrete;
using BusinessLayer.ValidationRules;
using DataAccessLayer.EntityFramework;
using EntityLayer.Concrete;
using FluentValidation;
using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Cafe_Menu.Controllers
{
    public class ProductController : Controller
    {
        ProductManager pm = new ProductManager(new EfProductDal());
        CategoryManager cm = new CategoryManager(new EfCategoryDal());

        // GET: Product
        [HttpGet]
        public ActionResult Index(int categoryID)
        {
            var values = pm.GetProductByCategory(categoryID);
            return View(values);
        }
        [Authorize]
        [HttpGet]
        public ActionResult ProductAdd()
        {
            List<SelectListItem> valueproduct = (from x in cm.GetList()
                                                 select new SelectListItem
                                                 {
                                                     Text = x.CategoryName,
                                                     Value = x.CategoryID.ToString(),
                                                 }).ToList();
            ViewBag.vlp = valueproduct;
            return View();
        }
        [Authorize]
        [HttpPost]
        public ActionResult ProductAdd(Product p, HttpPostedFileBase ProductImage)
        {
            {
                ProductValidator productValidator = new ProductValidator();
                ValidationResult results = productValidator.Validate(p);
                p.ProductStatus = true;

                if (results.IsValid)
                {
                    if (ProductImage != null && ProductImage.ContentLength > 0)
                    {
                        try
                        {
                            var existingCategory = pm.GetList().FirstOrDefault(x => x.ProductID == p.ProductID);
                            if (existingCategory != null && !string.IsNullOrEmpty(existingCategory.ProductImage))
                            {
                                string oldImagePath = Server.MapPath(existingCategory.ProductImage);
                                if (System.IO.File.Exists(oldImagePath))
                                {
                                    string oldHash = FileHelper.ComputeFileHash(oldImagePath);
                                    string newHash = FileHelper.ComputeFileHash(ProductImage);

                                    if (oldHash != newHash)
                                    {
                                        System.IO.File.Delete(oldImagePath);
                                    }
                                }
                            }

                            string filePath = Path.Combine(Server.MapPath("~/Uploads/"), ProductImage.FileName);
                            ProductImage.SaveAs(filePath);

                            p.ProductImage = "/Uploads/" + ProductImage.FileName;
                        }
                        catch (Exception ex)
                        {
                            ModelState.AddModelError("", "Resim yüklenirken hata oluştu: " + ex.Message);
                        }
                    }

                    // Veritabanına kategori ekle
                    pm.ProductAdd(p);
                    return RedirectToAction("ProductAdd");
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
        }
        [Authorize]
        [HttpGet]
        public ActionResult Products()
        {
            var products = pm.GetList();
            return View(products);
        }
        [Authorize]
        [HttpGet]
        public ActionResult ProductEdit(int id)
        {
            var productvalues = pm.GetProductById(id);
            return View(productvalues);
        }
        [Authorize]
        [HttpPost]
        public ActionResult ProductEdit(int id, Product p, HttpPostedFileBase ProductImage)
        {
            var existingProduct = pm.GetProductById(id);
            if(existingProduct == null)
            {
                return HttpNotFound();
            }

            ProductValidator productValidator = new ProductValidator();
            ValidationResult results = productValidator.Validate(p);

            if (results.IsValid)
            {
                existingProduct.ProductName = p.ProductName;
                existingProduct.ProductDescription = p.ProductDescription;
                existingProduct.ProductPrice = p.ProductPrice;

                if (ProductImage != null && ProductImage.ContentLength > 0)
                {
                    try
                    {
                        if (!string.IsNullOrEmpty(existingProduct.ProductImage))
                        {
                            string oldImagePath = Server.MapPath(existingProduct.ProductImage);
                            if (System.IO.File.Exists(oldImagePath))
                            {
                                System.IO.File.Delete(oldImagePath);
                            }
                        }

                        string filePath = Path.Combine(Server.MapPath("~/Uploads/"), ProductImage.FileName);
                        ProductImage.SaveAs(filePath);
                        existingProduct.ProductImage = "/Uploads/" + ProductImage.FileName;
                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError("", "Resim yüklenirken hata oluştu: " + ex.Message);
                    }
                }

                pm.ProductUpdate(existingProduct);
                return RedirectToAction("Products");
            }
            else
            {
                foreach ( var item in results.Errors)
                {
                    ModelState.AddModelError(item.PropertyName, item.ErrorMessage);
                }
            }
            return View(existingProduct);
        }
        [Authorize]
        [HttpPost]
        public JsonResult ToggleStatus(int id)
        {
            var product = pm.GetProductById(id);
            if (product == null)
            {
                return Json(new { success = false });
            }

            // Toggle the product status
            product.ProductStatus = !product.ProductStatus;

            // Update the product status in the database
            pm.ProductUpdate(product);

            return Json(new { success = true, newStatus = product.ProductStatus });
        }
        [Authorize]
        [HttpPost]
        public JsonResult ProductDelete(int id)
        {
            var product = pm.GetProductById(id);
            if (product == null)
            {
                return Json(new { success = false });
            }

            // Delete the product from the database
            pm.ProductDelete(product);

            return Json(new { success = true });
        }
    }
}