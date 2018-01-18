using MultiActiveSorbDirectory.Models;
using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace MultiActiveSorbDirectory.Controllers
{
    public class CreateUserController : Controller
    {
        List<Person> people = new List<Person>();

        // GET: CreateUser
        public ActionResult Index()
        {
            return View();
        }

        private bool checkErrors(Account m)
        {
            if (m.c == null) { return true; }
            if (m.department == null) { return true; }
            if (m.employeeID == null) { return true; }
            if (m.givenName == null) { return true; }
            if (m.initials == null) { return true; }
            if (m.l == null) { return true; }
            if (m.mail == null) { return true; }
            if (m.manager == null) { return true; }
            if (m.mobile == null) { return true; }
            if (m.postalCode == null) { return true; }
            if (m.sAMAccountName == null) { return true; }
            if (m.SN == null) { return true; }
            if (m.st == null) { return true; }
            if (m.streetAddress == null) { return true; }
            if (m.telephoneNumber == null) { return true; }
            if (m.title == null) { return true; }
            return false ;
        }

        private string getDistinguishedName(String user, DirectoryEntry de)
        {
            DirectorySearcher searcher = new DirectorySearcher(de)
            {
                PageSize = int.MaxValue,
                Filter = "(&(objectCategory=person)(objectClass=user)(sAMAccountName=" + user + "))"
            };

            var result = searcher.FindOne();

            if (result == null)
            {
                return "";
            }
            try
            {
                string DN = "";

                if (result.Properties.Contains("distinguishedName"))
                {
                    DN = result.Properties["distinguishedName"][0].ToString();
                }
                return DN;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return "";
            }
        }

        private bool createUserNow(Account m)
        {
            // connect to LDAP  

            DirectoryEntry myLdapConnection = new DirectoryEntry("LDAP://OU=Users,OU=Harlem Road,DC=multisorb,DC=com", "Administrator", "325H@l3m!");

            // define vars for user  

            DirectoryEntry user = myLdapConnection.Children.Add(
                                 "CN=" + m.givenName + " " + m.SN, "user");

            // User name (domain based)   
            user.Properties["userprincipalname"].Add(m.sAMAccountName + "@multisorb.com");

            // User name (older systems)  
            user.Properties["samaccountname"].Add(m.sAMAccountName);

            // Surname  
            user.Properties["sn"].Add(m.SN);

            // Forename  
            user.Properties["givenname"].Add(m.givenName);

            // Display name
            user.Properties["displayname"].Add(m.SN + ", " + m.givenName + " " + m.initials + ".");

            // E-mail  
            user.Properties["mail"].Add(m.mail + "@multisorb.com");

            //Country
            user.Properties["c"].Add(m.c);

            //title
            user.Properties["title"].Add(m.title);

            //Department
            user.Properties["department"].Add(m.department);

            //Telephone number
            user.Properties["telephoneNumber"].Add(m.telephoneNumber);

            //Street
            user.Properties["streetAddress"].Add(m.streetAddress);

            //City
            user.Properties["l"].Add(m.l);

            //State
            user.Properties["st"].Add(m.st);

            //Postal Code
            user.Properties["postalCode"].Add(m.postalCode);

            

            //Mobile Phone
            user.Properties["mobile"].Add(m.mobile);

            //Company
            user.Properties["company"].Add("Multisorb");


            //Manager
            String managerDN = getDistinguishedName(m.manager, myLdapConnection);
            if (managerDN == "")
            {
                return false;
            }
            else
            {
                user.Properties["manager"].Add(managerDN);
            }

            try
            {
                //commit the property changes
                user.CommitChanges();

                //Logon Script //might be breaking
                user.Properties["scriptPath"].Add("login.vbs");

                //EmployeeID //might be breaking
                user.Properties["employeeID"].Add(m.employeeID);

                //commit the property changes
                user.CommitChanges();

                // set user's password 
                user.Invoke("SetPassword", "Mti@325");

                //enable account
                user.Invoke("Put", new object[] { "userAccountControl", "512" });
                return true;
            }
            catch(Exception e)
            {
                Console.WriteLine(e.ToString());
                return false;
            }
            
        }

        [HttpPost]
        public JsonResult checkAlias(String alias)
        {
            if (alias == "")
            {
                return Json(new { success = false, error = "Alias cannot be blank" }, JsonRequestBehavior.AllowGet);
            }
            try
            {
                DirectoryEntry myLdapConnection = new DirectoryEntry("LDAP://OU=Users,OU=Harlem Road,DC=multisorb,DC=com", "Administrator", "325H@l3m!");

                DirectorySearcher search = new DirectorySearcher(myLdapConnection);
                search.Filter = "(sAMAccountName=" + alias + ")";
                SearchResult result = search.FindOne();

                if (result == null)
                {
                    return Json(new { success = true }, JsonRequestBehavior.AllowGet);
                }
                else return Json(new { success = false, error="Alias taken already" }, JsonRequestBehavior.AllowGet);
            }

            catch (Exception e)
            {
                return Json(new { success = false, error = "Show this to an IT person please: "+e.ToString() }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult checkEmail(String mail)
        {
            try
            {
                DirectoryEntry myLdapConnection = new DirectoryEntry("LDAP://OU=Users,OU=Harlem Road,DC=multisorb,DC=com", "Administrator", "325H@l3m!");


                DirectorySearcher search = new DirectorySearcher(myLdapConnection);
                search.Filter = "(mail=" + mail + ")";
                SearchResult result = search.FindOne();

                if (result == null)
                {
                    return Json(new { success = true }, JsonRequestBehavior.AllowGet);
                }
                else return Json(new { success = false, error = "Email address taken already" }, JsonRequestBehavior.AllowGet);
            }

            catch (Exception e)
            {
                return Json(new { success = false, error = "Show this to an IT person please: " + e.ToString() }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult Index(Account obj)
        {
            if (checkErrors(obj))
            {
                return View();
            }
            else
            {
                if (createUserNow(obj))
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    return View();
                }
            }
        }

        [HttpPost]
        public string populateSupervisors()
        {
            DirectoryEntry de = new DirectoryEntry("LDAP://OU=Users,OU=Harlem Road,DC=multisorb,DC=com", "Administrator", "325H@l3m!");

            ActiveDirectoryAccount viewModel = new ActiveDirectoryAccount();
            List<Account> allUsersView = new List<Account>();

            DirectorySearcher userSearcher = new DirectorySearcher(de);
            userSearcher.SearchScope = SearchScope.OneLevel;
            userSearcher.Filter = "(&(objectCategory=person)(objectClass=user))";
            foreach (SearchResult user in userSearcher.FindAll())
            {
                Person inquestion = new Person();
                inquestion.displayName = user.Properties["displayName"][0].ToString();
                inquestion.sAMAccountName = user.Properties["sAMAccountName"][0].ToString();
                people.Add(inquestion);
            }
            JavaScriptSerializer ser = new JavaScriptSerializer();
            return ser.Serialize(people);
        }

    }
}