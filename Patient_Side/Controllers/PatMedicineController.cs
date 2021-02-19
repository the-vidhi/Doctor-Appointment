using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Medical.Models;
using Patient_Side.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using System.Dynamic;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.SqlServer;
using Microsoft.SqlServer;
using Microsoft.AspNetCore.Session;
using System.Web;
using cloudscribe.Pagination.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Transactions;
using System.Data;
using System.Data.SqlClient;

namespace Patient_Side.Controllers
{
    public class PatMedicineController : Controller
    {
        //private readonly PatientContext _context;
        private readonly MedContext _context1;


        public PatMedicineController(MedContext context1)
        {
            // _context = context;
            _context1 = context1;
        }
        public IActionResult Shop(string cateSort, string searchString, string sortOrder, int pageNumber = 1, int pageSize = 9)
        {

            ViewBag.PriceSortParam = string.IsNullOrEmpty(sortOrder) ? "Price_Desc" : "";
            ViewBag.CurrentFilter = searchString;
            ViewBag.CateFilter = cateSort;
            int records = (pageSize * pageNumber) - pageSize;


            var medList = from m in _context1.MEDICINETB select m;
            var Count = medList.Count();
            ViewBag.Count = medList.Count();


            if (!string.IsNullOrEmpty(searchString))
            {
                medList = medList.Where(m => m.Medicine_Name.Contains(searchString));
                Count = medList.Count();
            }
            if (!string.IsNullOrEmpty(cateSort))
            {
                medList = medList.Where(m => m.Medicine_Content.Contains(cateSort));
                Count = medList.Count();
            }
            //if((!string.IsNullOrEmpty(cateSort)) && (ViewBag.PriceSortParam == "Price_Desc"))
            //{
            //    medList = medList.Where(m => m.Medicine_Content.Contains(cateSort)).OrderByDescending(m => m.Medicine_Price);
            //}
            switch (sortOrder)
            {
                case "Price_Desc":
                    medList = medList.OrderByDescending(m => m.Medicine_Price);
                    break;
                default:
                    medList = medList.OrderBy(m => m.Medicine_Price);
                    break;
            }

            medList = medList
                .Skip(records).Take(pageSize);
            var result = new PagedResult<Medicine>
            {
                Data = medList.AsNoTracking().ToList(),
                TotalItems = Count,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
            //return View(medList.ToList());

            TempData.Keep("SessionID");
            return View(result);
        }



        // [Route("PatMedicine/ShopDetails/{id}")]
        public IActionResult ShopDetails(int id)
        {
            var medlist = _context1.MEDICINETB.FirstOrDefault(m => m.Medicine_ID == id);
            ViewModel vm = new ViewModel();
            vm.ProductReviewList = _context1.PRODUCTREVIEWTB.ToList();

            vm.Medi_Id = id;
            vm.medicine = medlist;
            if (medlist == null)
            {
                TempData.Keep("SessionID");
                return View("NotFound");
            }
            TempData.Keep("SessionID");
            return View(vm);
        }

        public IActionResult RandomMedicinesPartial()
        {
            var medList = _context1.MEDICINETB.ToList();
            //var random = new Random();
            //medList = medList.OrderBy(product => random.Next()).Where(p => p.Medicine_Content = 's' );

            //medList = medList.OrderBy(product => SqlFunctions.Rand().Limit(5));

            //var query = "select * from MEDICINETB";

            TempData.Keep("SessionID");
            return View(medList);
        }
        public IActionResult RelatedMedicinesPartial()
        {
            var medList = from d in _context1.MEDICINETB where d.Medicine_Content == "Paracetamol"
                          select new Medicine
                          {
                              Medicine_ID = d.Medicine_ID,
                              Medicine_Image = d.Medicine_Image,
                              Medicine_Price = d.Medicine_Price,
                              Medicine_Name = d.Medicine_Name
                          };

            TempData.Keep("SessionID");
            return View(medList);
        }

        [HttpGet]
        public IActionResult ProductReviewAddPartial()
        {
            var vm = new ViewModel();

            var pt = from r in _context1.PRODUCTREVIEWTB
                     join p in _context1.PATIENTTB on r.Patient_ID equals p.Patient_ID
                     select new ProductReview
                     {
                         Product_Review_ID = r.Product_Review_ID,
                         Product_Review_Msg = r.Product_Review_Msg,
                         Patient_ID = r.Patient_ID,
                         Patient_Name = p.Patient_Name,
                         Patient_Profile = p.Patient_Profile,
                         Medicine_ID = r.Medicine_ID
                     };
            vm.ProductReviewList = pt.ToList();

            TempData.Keep("SessionID");
            return View(vm);
        }
        [HttpPost]
        public IActionResult ProductReviewAddPartial(ViewModel vm)
        {
            if (HttpContext.Session.GetString("SessionID") == null)
            {
                return RedirectToAction("Login", "PatientReg");
            }
            // review.Medi_Id = review.productReviewData.Medicine_ID;
            vm.productReviewData.Patient_ID = (int)TempData["SessionID"];
            int ID = Convert.ToInt32(Request.Form["Id"]);
            vm.productReviewData.Medicine_ID = ID;


            _context1.PRODUCTREVIEWTB.Add(vm.productReviewData);
            _context1.SaveChanges();

            TempData.Keep("SessionID");
            return RedirectToAction("Shop");
        }

        //public IActionResult AddToCart()
        //{
        //    TempData.Keep("SessionID");
        //    return View();
        //}
       
        public async Task <IActionResult> AddToCart(Cart cart)
        {
            if (HttpContext.Session.GetString("SessionID") == null)
            {
                return RedirectToAction("Login", "PatientReg");
            }
            // ViewModel vm = new ViewModel();
            var pID = (int)TempData["SessionID"];
            var checkMed = _context1.CARTTB.FirstOrDefault(x => x.Patient_ID == pID);
            
            

            cart.Patient_ID = (int)TempData["SessionID"];
            cart.Cart_Qty = 1;
            ///vm.cart.Medicine_ID = vm.Medi_Id;
            //cart.Medicine_ID = id;

            int ID = Convert.ToInt32(Request.Form["Id"]);
            cart.Medicine_ID = ID;

            if(checkMed != null)
            {
                if (checkMed.Medicine_ID == ID)
                {
                    TempData.Keep("SessionID");
                    return RedirectToAction("Shop", "PatMedicine");
                }
            }
            

            _context1.CARTTB.Add(cart);
            _context1.SaveChanges();

            TempData.Keep("SessionID");
            return RedirectToAction("Shop", "PatMedicine");
        }
        public IActionResult CartDisplay()
        {
            if (HttpContext.Session.GetString("SessionID") == null)
            {
                return RedirectToAction("Login", "PatientReg");
            }
            var sid = HttpContext.Session.GetInt32("SessionID");
            // var sid = (int)TempData["sid"];
            var cList = from c in _context1.CARTTB
                        join p in _context1.PATIENTTB on c.Patient_ID equals p.Patient_ID
                        join m in _context1.MEDICINETB on c.Medicine_ID equals m.Medicine_ID
                        where p.Patient_IsActive == true && p.Patient_ID == sid

                        select new Cart
                        {
                            Cart_ID = c.Cart_ID,
                            Cart_Qty = c.Cart_Qty,
                            Medicine_ID = c.Medicine_ID,
                            Patient_ID = c.Patient_ID,
                            Medicine_Name = m.Medicine_Name,
                            Medicine_Price = m.Medicine_Price,
                            Medicine_Image = m.Medicine_Image

                        };
            var cList1 = _context1.CARTTB.FirstOrDefault(x => x.Patient_ID == sid);
            // @TempData["qty"] = cart.Medicine_Price * cart.Cart_Qty;

            if (cList1 == null)
            {
                TempData.Keep("SessionID");
                return RedirectToAction("EmptyCart", "PatMedicine");
                
            }
            TempData.Keep("SessionID");
            return View(cList);
        }
        public IActionResult EmptyCart()
        {
            TempData.Keep("SessionID");
            return View();
        }
        public async Task<IActionResult> CartDelete(int id)
        {
            var cartDel = await _context1.CARTTB.FindAsync(id);
            _context1.CARTTB.Remove(cartDel);
            await _context1.SaveChangesAsync();
            // HttpContext.Session.Clear();

            TempData.Keep("SessionID");
            return RedirectToAction("CartDisplay", "PatMedicine");

        }
        //[HttpPost]
        //public async Task<IActionResult> CartUpdate(Cart cart)
        //{
        //    //string[] quantities = Request.Form["quantity"];
        //    //List<Cart> cart1 = (List<Cart>)TempData["cart1"];
        //    //for (int i = 0; i < cart1.Count; i++)
        //    //{
        //    //    cart1[i].Cart_Qty = Convert.ToInt32(quantities[i]);
        //    //}
        //    //TempData["cart1"] = cart1;
            
           
        //    cart.Cart_Qty = int.Parse(Request.Form["quantity"]);
        //    cart.Cart_ID = int.Parse(Request.Form["Id"]);
        //    cart.Medicine_ID = int.Parse(Request.Form["MedId"]);
        //    if (ModelState.IsValid)
        //    {
        //        _context1.Update(cart);
        //        await _context1.SaveChangesAsync();

        //        //return RedirectToAction(nameof(Index));
        //    }

        //    TempData.Keep("SessionID");
        //    return RedirectToAction("CartDisplay");
        //}
        [Route("PatMedicine/Increase/{item_code}")]
        public ActionResult Increase(int item_code)
        {

            //TempData["role"] = Session["role"].ToString();
            Cart i = _context1.CARTTB.Where(x => x.Cart_ID == item_code).SingleOrDefault();
            i.Cart_Qty = i.Cart_Qty + 1;
            _context1.SaveChanges();

            TempData.Keep("SessionID");
            return RedirectToAction("CartDisplay","PatMedicine");

        }

        [Route("PatMedicine/Decrease/{item_code}")]
        public ActionResult Decrease(int item_code)
        {
           // TempData["role"] = Session["role"].ToString();
            Cart i = _context1.CARTTB.Where(x => x.Cart_ID == item_code).SingleOrDefault();
            i.Cart_Qty = i.Cart_Qty - 1;
            _context1.SaveChanges();

            TempData.Keep("SessionID");
            return RedirectToAction("CartDisplay", "PatMedicine");

        }
        public ActionResult Checkout()
        {
            if (HttpContext.Session.GetString("SessionID") == null)
            {
                return RedirectToAction("Login", "PatientReg");
            }
            var sid = HttpContext.Session.GetInt32("SessionID");

            var cList1 = _context1.CARTTB.FirstOrDefault(x => x.Patient_ID == sid);
            if (cList1 == null)
            {
                TempData.Keep("SessionID");
                return RedirectToAction("EmptyCart", "PatMedicine");

            }
            //var id = (int)TempData["SessionID"];
            if (sid == 0)
            {
                TempData.Keep("SessionID");
                return NotFound();
            }
            var vm = new ViewModel();
           // vm.cartList = _context1.CARTTB.Where(m => m.Patient_ID == id).ToList();

            var mList = from c in _context1.CARTTB
                        join m in _context1.MEDICINETB on c.Medicine_ID equals m.Medicine_ID
                        where c.Patient_ID == sid

                        select new Cart
                        {
                            Cart_ID = c.Cart_ID,
                            Cart_Qty = c.Cart_Qty,
                            Medicine_ID = c.Medicine_ID,
                            Patient_ID = c.Patient_ID,
                            Medicine_Price = m.Medicine_Price,
                            Medicine_Image = m.Medicine_Image,
                            Medicine_Name = m.Medicine_Name,

                        };

            vm.cartList = mList.ToList();

            vm.patient = _context1.PATIENTTB.Find(sid);
           

            vm.paymentList = _context1.PAYMENTTB.ToList();

            if (vm.cartList == null)
            {
                TempData.Keep("SessionID");
                return NotFound();
            }
            List<State> statelist = new List<State>();
            statelist = (from statetb in _context1.STATETB
                         select statetb).ToList();
            statelist.Insert(0, new State { State_ID = 0, State_Name = "Select State" });
            ViewBag.ListofState = statelist;


            TempData.Keep("SessionID");
            return View(vm);
        }

        [HttpPost]
        public ActionResult Checkout(Order order,Bill bill)
        {
            var vm = new ViewModel();
            var id = (int)TempData["SessionID"];
            var ordamt = Convert.ToString(Request.Form["OrdAmt"]);
            var payID = Convert.ToInt32(Request.Form["payment-group"]);

            
            
            order.Patient_ID = id;
            order.Order_Amount = ordamt;
            order.Order_Date = Convert.ToString(DateTime.Now);
            order.Order_Status = "Confirm";
            order.Payment_Type_ID = payID;
            order.Deliver_Date = "31-jan-21 ";
            order.Cart_ID = 1;



            if (order.Payment_Type_ID == 1)
            {
                order.Is_Paid = false;
            }
            if(order.Payment_Type_ID == 3)
            {
               order.Is_Paid = true;
            }
            //if (ModelState.IsValid)
            //{
                _context1.ORDERTB.Add(order);
                _context1.SaveChanges();
            //}
            int oid = _context1.ORDERTB.Max(x => x.Order_ID);
            //int oid = 1;

            var cartdel = (from c in _context1.CARTTB
                           join md in _context1.MEDICINETB on c.Medicine_ID equals md.Medicine_ID
                           where c.Patient_ID == id
                           select new Cart
                           {
                               Cart_ID = c.Cart_ID,
                               Cart_Qty = c.Cart_Qty,
                               Medicine_ID = c.Medicine_ID,
                               Medicine_Name = md.Medicine_Name,
                               Medicine_Price = md.Medicine_Price,
                               Patient_ID = c.Patient_ID,
                               Medicine_Image = md.Medicine_Image
                           });
                            //irstOrDefault(m => m.Patient_ID == id);

                OrderMedical om = new OrderMedical();
            int omid = _context1.ORDER_MEDICALTB.Max(m => m.Order_Medicine_ID);
            int ordmedid = omid + 1;
            om.Order_Medicine_ID = ordmedid;
            foreach (var item in cartdel.ToList())
            {
                //var ProdTotal = Convert.ToString(Request.Form["ProdTotal"]);
                om.Order_Qty = item.Cart_Qty;
                om.Medicine_ID = item.Medicine_ID;

                var price = (int.Parse(item.Medicine_Price) * item.Cart_Qty);
                om.Price =  price.ToString();

                om.Order_ID = oid;
                
                _context1.ORDER_MEDICALTB.Add(om);
                _context1.SaveChanges();
               
                om.Order_Medicine_ID++;

            }
           
            

            if (bill.State_ID == 0)
            {
                ModelState.AddModelError("", "Select State");
            }
            else if (bill.City_ID == 0)
            {
                ModelState.AddModelError("", "Select City");
            }
            var SubCategoryID = HttpContext.Request.Form["City_ID"].ToString();
            List<State> statelist = new List<State>();
            statelist = (from state in _context1.STATETB
                         select state).ToList();
            statelist.Insert(0, new State { State_ID = 0, State_Name = "Select State" });
            ViewBag.ListofCategory = statelist;

            bill.Patient_ID = id;
            bill.Order_ID = oid;
            var PatName = Convert.ToString(Request.Form["bname"]);
            var BEmail = Convert.ToString(Request.Form["bemail"]);
            var BContact = Convert.ToString(Request.Form["bcontact"]);
            var BState = Convert.ToInt32(Request.Form["State_ID"]);
            var BCity = Convert.ToInt32(Request.Form["City_ID"]);
            var BAddress = Convert.ToString(Request.Form["baddress"]);
            var BPincode = Convert.ToString(Request.Form["bpincode"]);

            
            //_context1.BILLTB.Add(bill);
            //_context1.ORDER_MEDICALTB.RemoveRange();
            //_context1.SaveChanges();


            string con = @"Data Source=DESKTOP-SBQBM8M\SQLEXPRESS;Initial Catalog=SEM_7_PROJECT;Integrated Security=True";
            SqlConnection db = new SqlConnection(con);
            db.Open();
           // string insert = "insert into BILLTB(Patient_ID,Patient_Name,Bill_Email,Bill_Contact,Order_ID,State_ID,City_ID,Bill_Address,Bill_Pincode) values(id,PatName,BEmail,BContact,oid,BState,BCity,BAddress,BPincode)";
            string insert = "insert into BILLTB(Patient_ID,Patient_Name,Bill_Email,Bill_Contact,Order_ID,State_ID,City_ID,Bill_Address,Bill_Pincode) values("+id+",'"+PatName+"','"+BEmail+"',"+BContact+","+oid+","+BState+","+BCity+",'"+BAddress+"',"+BPincode+")";
            SqlCommand cmd = new SqlCommand(insert,db);
            int m = cmd.ExecuteNonQuery();

            string delete = "delete from CARTTB where Patient_ID = "+id+"";
            SqlCommand cmd1 = new SqlCommand(delete, db);
            int m1 = cmd1.ExecuteNonQuery();

            db.Close();





            //_context1.CARTTB.Remove(cartdel);
            TempData.Keep("SessionID");
                return RedirectToAction("Success", "PatMedicine");      
          
        }
        
        public JsonResult GetSubCategory(int State_ID)
        {
            List<City> citylist = new List<City>();
            citylist = (from city in _context1.CITYTB
                        where city.State_ID == State_ID
                        select city).ToList();
            citylist.Insert(0, new City { City_ID = 0, City_Name = "Select City" });

            TempData.Keep("SessionID");
            return Json(new SelectList(citylist, "City_ID", "City_Name"));
        }

        public IActionResult OrderDetails(int id)
        {
            if (HttpContext.Session.GetString("SessionID") == null)
            {
                return RedirectToAction("Login", "PatientReg");
            }
            var sid = HttpContext.Session.GetInt32("SessionID");

            var oList = from om in _context1.ORDER_MEDICALTB
                        join o in _context1.ORDERTB on om.Order_ID equals o.Order_ID
                        join p in _context1.PATIENTTB on o.Patient_ID equals p.Patient_ID
                        join m in _context1.MEDICINETB on om.Medicine_ID equals m.Medicine_ID
                        where om.Order_ID == id && o.Patient_ID == sid && p.Patient_IsActive == true
                        select new OrderMedical
                        {
                            Order_Medicine_ID = om.Order_Medicine_ID,
                            Order_ID = om.Order_ID,
                            Order_Qty = om.Order_Qty,
                            Medicine_Name = m.Medicine_Name,
                            Medicine_Image = m.Medicine_Image,
                            Medicine_ID = om.Medicine_ID,
                            Price = om.Price,
                            Order_Amount = o.Order_Amount
                        };
            ViewModel vm = new ViewModel();
            vm.orderMedicalList = oList.ToList();

            var ord = from om in _context1.ORDER_MEDICALTB
                        join o in _context1.ORDERTB on om.Order_ID equals o.Order_ID
                        //where om.Order_ID == id
                        select new OrderMedical
                        {
                            Order_Medicine_ID = om.Order_Medicine_ID,
                            Order_ID = om.Order_ID,
                            Order_Qty = om.Order_Qty,
                            Medicine_ID = om.Medicine_ID,
                            Price = om.Price,
                            Order_Amount = o.Order_Amount
                        };
            vm.orderMedical = ord.FirstOrDefault(x => x.Order_ID == id);


            TempData.Keep("SessionID");
            return View(vm);
        }
        public IActionResult Success()
        {

            TempData.Keep("SessionID");
            return View();
        }
    }
}


