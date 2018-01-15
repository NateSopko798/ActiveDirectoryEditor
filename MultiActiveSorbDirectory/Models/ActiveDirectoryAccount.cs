using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MultiActiveSorbDirectory.Models
{
    public class Account
    {
        public string distinguishingName { get; set; } //DN is simply the most important LDAP attribute. CN=Jay Jamieson, OU = Newport, DC = cp, DC = com
        public string CN { get; set; } //Maps to 'Name' in the LDAP provider. Remember CN is a mandatory property.  See also sAMAccountName.
        public string displayName { get; set; } //displayName = Guy Thomas.  If you script this property, be sure you understand which field you are configuring.  DisplayName can be confused with CN or description.
        public string givenName { get; set; } //Firstname also called Christian name
        public string initials { get; set; } //Useful in some cultures
        public string name { get; set; } //name = Guy Thomas.  Exactly the same as CN.
        public string objectCategory { get; set; } //Defines the Active Directory Schema category. For example, objectCategory = Person
        public string sAMAccountName { get; set; } //This is a mandatory property, sAMAccountName = guyt.  The old NT 4.0 logon name, must be unique in the domain. 
        public string SN { get; set; } //SN = Thomas. This would be referred to as last name or surname.
        public string title { get; set; } //Job title.  For example Manager.
        public string userPrincipalName { get; set; } //userPrincipalName = guyt@CP.com  Often abbreviated to UPN, and looks like an email address.  Very useful for logging on especially in a large Forest.  Note UPN must be unique in the forest.
        public string mail { get; set; } //An easy, but important attribute.  A simple SMTP address is all that is required billyn@ourdom.com
        public string mailNickname { get; set; } //Normally this is the same value as the sAMAccountName, but could be different if you wished.  Needed for mail enabled contacts.
        public string msExchHomeServerName { get; set; } //Exchange needs to know which server to deliver the mail.  Example: /o=YourOrg/ou=First Administrative Group/cn=Configuration/cn=Servers/cn=MailSrv
        public string showInAddressBook { get; set; } //Displays the contact in the Global Address List. Example:CN=All Users,CN=All Address Lists,CN=Address Lists Container,CN=Multisorb,CN=Microsoft Exchange,CN=Services,CN=Configuration,DC=multisorb,DC=com or CN=Default Global Address List,CN=All Global Address Lists,CN=Address Lists Container,CN=Multisorb,CN=Microsoft Exchange,CN=Services,CN=Configuration,DC=multisorb,DC=com
        public string c { get; set; } //Country or Region
        public string company { get; set; } //Company or organization name
        public string department { get; set; } //Useful category to fill in and use for filtering
        public string homephone { get; set; } //Home Phone number, (Lots more phone LDAPs)
        public string l { get; set; } //L = Location.  City ( Maybe Office
        public string manager { get; set; } //Boss, manager
        public string mobile { get; set; } //Mobile Phone number
        public string ObjectClass { get; set; } //Usually, User, or Computer
        public string pwdLastSet { get; set; } //Force users to change their passwords at next logon
        public string postalCode { get; set; } //Zip or post code
        public string physicalDeliveryOfficeName { get; set; }
        public string st { get; set; } //State, Province or County
        public string employeeID { get; set; } //for printing - user attribute
        public string streetAddress { get; set; } //First line of address
        public string telephoneNumber { get; set; } //Office Phone
        public string userAccountControl { get; set; } //Enable (512) / disable account (514)
    }

    public class Person
    {
        public string displayName { get; set; }
        public string sAMAccountName { get; set; }
    }

    public class ActiveDirectoryAccount
    {
        public List<Account> Accounts = new List<Account>();
    }
}
