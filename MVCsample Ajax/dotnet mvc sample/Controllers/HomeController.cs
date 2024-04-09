using Syncfusion.EJ2.Base;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace dotnet_mvc_sample.Controllers
{
    public class HomeController : Controller
    {
        public static List<columnC> Column = new List<columnC>();
        public ActionResult Index()
        {

            var Order = OrdersDetails.GetAllRecords();
            ViewBag.DataSource = OrdersDetails.GetAllRecords();
            ViewBag.dnList = OrdersData.GetAllRecords();
            ViewBag.DataSource1 = OrdersData.GetAllRecords();
            ViewBag.DataSource2 = OrdersDetails.GetAllRecords();
            ViewBag.DataSource3 = BigData.GetAllRecords();
            ViewBag.columnData = Column; // stored column data
            return View();
        }
        [HttpPost]
       
        public ActionResult ColumnSettings(List<columnC> dm)
        {
            Column = dm; //column data
           
            return Json(new { result = dm });
        }
        public class FormDetails
        {
            public string FirstName { get; set; }
            public string LastName { get; set; }
        }

        public class CustomDataModel
        {
            public CustomDataModel()
            {
            }
            public List<OrdersDetails> NormalGridData { get; set; }
            public FormDetails FormData { get; set; }
        }

        public ActionResult FormSubmitGridData(CustomDataModel data)
        {
            return Json(data);
        }
        public class columnC
        {
            public string field { get; set; }
            public Boolean visible { get; set; }
            public string width { get; set; }
            public int index { get; set; }
        }
        public ActionResult TodayPage()
        {
            var jsontext = new { actPage = 4, actDay = 3 };
            return Json(jsontext, JsonRequestBehavior.AllowGet);
        }
        public ActionResult BatchUpdate(string action, CRUDModel batchmodel)
        {
            if (batchmodel.Changed != null)
            {
                for (var i = 0; i < batchmodel.Changed.Count(); i++)
                {
                    var ord = batchmodel.Changed[i];
                    OrdersData val = OrdersData.GetAllRecords().Where(or => or.UnitID == ord.UnitID).FirstOrDefault();
                    val.UnitID = ord.UnitID;
                   // val.ShipName = ord.ShipName;
                  //  val.EmployeeID = ord.EmployeeID;
                    val.N1 = ord.N1;
                    val.N2 = ord.N2;
                    val.ShipAddress = ord.ShipAddress;
                }
            }

            if (batchmodel.Deleted != null)
            {
                for (var i = 0; i < batchmodel.Deleted.Count(); i++)
                {
                    OrdersData.GetAllRecords().Remove(OrdersData.GetAllRecords().Where(or => or.UnitID == batchmodel.Deleted[i].UnitID).FirstOrDefault());
                }
            }

            if (batchmodel.Added != null)
            {
                for (var i = 0; i < batchmodel.Added.Count(); i++)
                {
                    OrdersData.GetAllRecords().Insert(0, batchmodel.Added[i]);
                }
            }
            var data = OrdersData.GetAllRecords().ToArray();
            return Json(new { added = batchmodel.Added, changed = batchmodel.Changed, deleted = batchmodel.Deleted, value = batchmodel.Value, action = batchmodel.action, key = batchmodel.key }); ;

        }
        public class CRUDModel
        {
            public List<OrdersData> Added { get; set; }
            public List<OrdersData> Changed { get; set; }
            public List<OrdersData> Deleted { get; set; }
            public OrdersData Value { get; set; }
            public int key { get; set; }
            public string action { get; set; }
        }
        public ActionResult UrlDatasourceDd1(DataManagerRequest dm)
        {
            IEnumerable<OrdersData> DataSource = OrdersData.GetAllRecords();
            DataOperations operation = new DataOperations();

            if (dm.Where != null && dm.Where.Count > 0) //Filtering
            {
                //DataSource = operation.PerformFiltering(DataSource, dm.Where, dm.Where[0].Operator);
                //if (dm.Where != null)
                //{
                //    DataSource = (from cust in DataSource
                //                  where cust.ShipName.ToLower().StartsWith(dm.Where[0].predicates[0].value.ToString())
                //            select cust).ToList();
                //}
            }
            if (dm.Skip != 0)
            {
                DataSource = operation.PerformSkip(DataSource, dm.Skip);   //Paging
            }
            if (dm.Take != 0)
            {
                DataSource = operation.PerformTake(DataSource, dm.Take);
            }
            return Json(DataSource);
        }
        public ActionResult ForeignKeyData(TestDm dm)
        {

            IEnumerable DataSource = OrdersData.GetAllRecords().ToList();
            DataOperations operation = new DataOperations();

            if (dm.Search != null && dm.Search.Count > 0)
            {
                DataSource = operation.PerformSearching(DataSource, dm.Search);  //Search
            }
            if (dm.Sorted != null && dm.Sorted.Count > 0) //Sorting
            {
                DataSource = operation.PerformSorting(DataSource, dm.Sorted);
            }
            if (dm.Where != null && dm.Where.Count > 0) //Filtering
            {
                DataSource = operation.PerformFiltering(DataSource, dm.Where, dm.Where[0].Operator);
            }
            int count = DataSource.Cast<OrdersData>().Count();
            if (dm.Skip != 0)
            {
                DataSource = operation.PerformSkip(DataSource, dm.Skip);   //Paging
            }
            if (dm.Take != 0)
            {
                DataSource = operation.PerformTake(DataSource, dm.Take);
            }
            return dm.RequiresCounts ? Json(new { result = DataSource, count = count }) : Json(DataSource);
        }
        public ActionResult UrlDatasource(DataManagerRequest dm)
        {
            IEnumerable<OrdersDetails> DataSource = OrdersDetails.GetAllRecords().ToList();
            DataOperations operation = new DataOperations();

            if (dm.Search != null && dm.Search.Count > 0)
            {
                DataSource = operation.PerformSearching(DataSource, dm.Search);  //Search
            }
            if (dm.Sorted != null && dm.Sorted.Count > 0) //Sorting
            {
                DataSource = operation.PerformSorting(DataSource, dm.Sorted);
            }
            if (dm.Where != null && dm.Where.Count > 0) //Filtering
            {
                DataSource = operation.PerformFiltering(DataSource, dm.Where, dm.Where[0].Operator);
            }
            int count = DataSource.Cast<OrdersDetails>().Count();
            if (dm.Skip != 0)
            {
                DataSource = operation.PerformSkip(DataSource, dm.Skip);   //Paging
            }
            if (dm.Take != 0)
            {
                DataSource = operation.PerformTake(DataSource, dm.Take);
            }
            return dm.RequiresCounts ? Json(new { result = DataSource, count = count }) : Json(DataSource);
        }

        public ActionResult UrlDatasourceChild(TestDm dm)
        {
            IEnumerable<BigData> DataSource = BigData.GetAllRecords().ToList();
            DataOperations operation = new DataOperations();

            if (dm.Search != null && dm.Search.Count > 0)
            {
                DataSource = operation.PerformSearching(DataSource, dm.Search);  //Search
            }
            if (dm.Sorted != null && dm.Sorted.Count > 0) //Sorting
            {
                DataSource = operation.PerformSorting(DataSource, dm.Sorted);
            }
            if (dm.Where != null && dm.Where.Count > 0) //Filtering
            {
                DataSource = operation.PerformFiltering(DataSource, dm.Where, dm.Where[0].Operator);
            }
            int count = DataSource.Cast<BigData>().Count();
            if (dm.Skip != 0)
            {
                DataSource = operation.PerformSkip(DataSource, dm.Skip);   //Paging
            }
            if (dm.Take != 0)
            {
                DataSource = operation.PerformTake(DataSource, dm.Take);
            }
            return dm.RequiresCounts ? Json(new { result = DataSource, count = count }) : Json(DataSource);
        }
        public class TestDm : DataManagerRequest
        {
            public ParamObject Params { get; set; }
            public int OrderID { get; set; }
            public string CustomerID { get; set; }
            public string ShipCountry { get; set; }
            public string ShipCity { get; set; }

        }
        public class ParamObject
        {
            public int OrderID { get; set; }
            public string CustomerID { get; set; }
            public string ShipCountry { get; set; }
            public string ShipCity { get; set; }
        }
        public class FilterQuery
        {

            public string Field { get; set; }
            public string Operator { get; set; }
            public string Value { get; set; }
        }
        

        public class ICRUDModel<T> where T : class
        {
            public string action { get; set; }

            public string table { get; set; }

            public string keyColumn { get; set; }

            public object key { get; set; }

            public T value { get; set; }

            public List<T> added { get; set; }

            public List<T> changed { get; set; }

            public List<T> deleted { get; set; }

            public IDictionary<string, object> @params { get; set; }
        }

 
        public class OrdersDetails
        {
            public static List<OrdersDetails> order = new List<OrdersDetails>();
            public OrdersDetails()
            {

            }
            public OrdersDetails(int EmployeeID, string Role, int Salary, string Address)
            {
                this.EmployeeID = EmployeeID;
                this.Role = Role;
                this.Salary = Salary;
                this.Address = Address;
            }
            public static List<OrdersDetails> GetAllRecords()
            {
                if (order.Count() == 0)
                {
                    int code = 20000;
                    int cd = 10;
                    for (int i = 1; i < 2; i++)
                    {
                        order.Add(new OrdersDetails(1, "TeamLead", 15000, "Chennai"));
                        order.Add(new OrdersDetails(2, "Manager", 20000, "Bangalore"));
                        order.Add(new OrdersDetails(3, "Engineer", 10000, "Cochin"));
                        order.Add(new OrdersDetails(4, "Sales", 20000, "Trivandrum"));
                        order.Add(new OrdersDetails(5, "Support", 10000, "Delhi"));
                        //order.Add(new OrdersDetails(code + 1, "ALFKI",  1, 2.3 * i, false, new DateTime(2023, 09, 01), "Berlin", "Simons bistro", "Denmark", new DateTime(1996, 7, 16), "Kirchgasse 6", false));
                        //order.Add(new OrdersDetails(code + 2, "ANTON",  2, 3.3 * i, true, new DateTime(2023, 10, 01), "Madrid", "Queen Cozinha", "Brazil", new DateTime(1996, 9, 11), "Avda. Azteca 123", true));
                        //order.Add(new OrdersDetails(code + 3, "IANTON",  3, 4.3 * i, true, new DateTime(2023, 09, 15), "Cholchester", "Frankenversand", "Germany", new DateTime(1996, 10, 7), "Carrera 52 con Ave. Bolívar #65-98 Llano Largo"));
                        //order.Add(new OrdersDetails(code + 4, "UBLONP",  4, 5.3 * i, false, new DateTime(2023, 10, 15), "Marseille", "Ernst Handel", "Austria", new DateTime(1996, 12, 30), "Magazinweg 7"));
                        //order.Add(new OrdersDetails(code + 5, "MBOLID", 5, 6.3 * i, true, new DateTime(2023, 02, 23), "Tsawassen", "Hanari Carnes", "Switzerland", new DateTime(1997, 12, 3), "1029 - 12th Ave. S."));
                        code += 5;
                        cd += 5;
                    }
                }
                return order;
            }

            public int? EmployeeID { get; set; }
            public string Role { get; set; }
            public int? Salary { get; set; }
            public string Address { get; set; }
        }

        public class OrdersData
        {
            public static List<OrdersData>
    order1 = new List<OrdersData>
        ();
            public OrdersData()
            {

            }
            public OrdersData(int UnitID, DateTime vchbilldate, int N2, int N1, int EmployeeID, int countbill, int netamt, int puramt, int grossprofit, int margin, int tax, int avgsale, string DnName, string ShipAddress)
            {
                this.UnitID = UnitID;
                this.N1 = N1;
                this.N2 = N2;
                this.EmployeeID = EmployeeID;
                this.vchbilldate = vchbilldate;
                this.countbill = countbill;
                this.netamt = netamt;
                this.puramt = puramt;
                this.grossprofit = grossprofit;
                this.margin = margin;
                this.tax = tax;
                this.avgsale = avgsale;
                this.DnName = DnName;
                this.ShipAddress = ShipAddress;
            }
            public static List<OrdersData>
                GetAllRecords()
            {
                if (order1.Count() == 0)
                {
                    int code = 10;
                    for (int i = 1; i < 2; i++)
                    {
                        order1.Add(new OrdersData(code + 1, new DateTime(2021, 04, 15), 8, 1,  1, 10, 20, 10, 20, 10, 20, 10, "Alizet", "Zen. Jelivia 12"));
                        order1.Add(new OrdersData(code + 2, new DateTime(2021, 04, 16), 10, 2,  2, 15, 25, 15, 25, 15, 25, 15, "Cozinha", "Avda. Azteca 123"));
                        order1.Add(new OrdersData(code + 3, new DateTime(2021, 04, 10), 15, 3,  3, 20, 30, 20, 30, 20, 30, 20, "Frankenversand", "Carrera 52 con Ave. Bolívar #65-98 Llano Largo"));
                        order1.Add(new OrdersData(code + 4, new DateTime(2021, 04, 9), 18, 4,  4, 25, 35, 2, 35, 25, 35, 25, "Handel", "Magazinweg 7"));
                        order1.Add(new OrdersData(code + 5, new DateTime(2021, 04, 7), 22, 5,  5, 30, 40, 30, 40, 30, 40, 30, "Carnes", "1029 - 12th Ave. S."));
                        order1.Add(new OrdersData(code + 6, new DateTime(2021, 04, 8), 18, 4, 4, 25, 35, 2, 35, 25, 35, 25, "Handel", "Magazinweg 7"));
                        order1.Add(new OrdersData(code + 7, new DateTime(2021, 04, 10), 22, 5, 5, 30, 40, 30, 40, 30, 40, 30, "Hizwat", "1029 - 12th Ave. S."));
                        code += 7;
                    }
                }
                return order1;
            }
            public int? UnitID { get; set; }
            public DateTime vchbilldate { get; set; }
            public int? EmployeeID { get; set; }
            public int? N1 { get; set; }
            public int? N2 { get; set; }
            public int? countbill { get; set; }
            public int? netamt { get; set; }
            public int? puramt { get; set; }
            public int? grossprofit { get; set; }
            public int? margin { get; set; }
            public int? tax { get; set; }
            public int? avgsale { get; set; }


            public string DnName { get; set; }
            public string ShipAddress { get; set; }
        }

        public class BigData
        {
            public static List<BigData> order = new List<BigData>();
            public BigData()
            {

            }
            public BigData(int OrderID, string CustomerId, int EmployeeId, double Freight, bool Verified, DateTime OrderDate, string ShipCity, string ShipName, string ShipCountry, DateTime ShippedDate, string ShipAddress)
            {
                this.OrderID = OrderID;
                this.CustomerID = CustomerId;
                this.EmployeeID = EmployeeId;
                this.Freight = Freight;
                this.ShipCity = ShipCity;
                this.Verified = Verified;
                this.OrderDate = OrderDate;
                this.ShipName = ShipName;
                this.ShipCountry = ShipCountry;
                this.ShippedDate = ShippedDate;
                this.ShipAddress = ShipAddress;
            }
            public static List<BigData> GetAllRecords()
            {
                if (order.Count() == 0)
                {
                    int code = 10000;
                    for (int i = 1; i < 2; i++)
                    {
                        order.Add(new BigData(code + 1, "ALFKI",  1, 2.3 * i, false, new DateTime(2023, 09, 01), "Berlin", "Simons bistro", "Denmark", new DateTime(1996, 7, 16), "Kirchgasse 6"));
                        order.Add(new BigData(code + 1, "ALFKI",  2, 3.3 * i, true, new DateTime(2023, 10, 01), "Madrid", "Queen Cozinha", "Brazil", new DateTime(1996, 9, 11), "Avda. Azteca 123"));
                        order.Add(new BigData(code + 3, "ANTON",  3, 4.3 * i, true, new DateTime(2023, 09, 15), "Cholchester", "Frankenversand", "Germany", new DateTime(1996, 10, 7), "Carrera 52 con Ave. Bolívar #65-98 Llano Largo"));
                        order.Add(new BigData(code + 4, "BLONP",  4, 5.3 * i, false, new DateTime(2023, 10, 15), "Marseille", "Ernst Handel", "Austria", new DateTime(1996, 12, 30), "Magazinweg 7"));
                        order.Add(new BigData(code + 5, "BLONP",  5, 6.3 * i, true, new DateTime(2023, 02, 23), "Tsawassen", "Hanari Carnes", "Switzerland", new DateTime(1997, 12, 3), "1029 - 12th Ave. S."));
                        code += 5;
                    }
                }
                return order;
            }
            public int? OrderID { get; set; }
            public string CustomerID { get; set; }
            public int? EmployeeID { get; set; }
            public double? Freight { get; set; }
            public string ShipCity { get; set; }
            public bool Verified { get; set; }
            public DateTime OrderDate { get; set; }
            public string ShipName { get; set; }
            public string ShipCountry { get; set; }
            public DateTime ShippedDate { get; set; }
            public string ShipAddress { get; set; }
        }
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}