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
            if (m.physicalDeliveryOfficeName == null) { return true; }
            if (m.postalCode == null) { return true; }
            if (m.sAMAccountName == null) { return true; }
            if (m.SN == null) { return true; }
            if (m.st == null) { return true; }
            if (m.streetAddress == null) { return true; }
            if (m.telephoneNumber == null) { return true; }
            if (m.title == null) { return true; }
            return false ;
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

            //commit the property changes
            user.CommitChanges();

            // set user's password  
            user.Invoke("SetPassword", "Mti@325");

            //enable account
            user.Invoke("Put", new object[] { "userAccountControl", "512" });
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

                return RedirectToAction("Index", "Home");
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