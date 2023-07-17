using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace MyCaseApi.Entities
{
    public class ApiDbContext : IdentityDbContext<IdentityUser>
    {
        public ApiDbContext()
        {

        }
        public ApiDbContext(DbContextOptions options) : base(options)
        {

        }
        public DbSet<User> User { get; set; }
        public DbSet<Service> Service { get; set; }
        public DbSet<PricePlan> PricePlan { get; set; }
        public DbSet<PackageService> packageService { get; set; }
        public DbSet<PaymentInfo> Payment { get; set; }
        public DbSet<AdminActivity> AdminActivity { get; set; }
        public DbSet<BillingMethod> BillingMethod { get; set; }
        public DbSet<CaseDetail> CaseDetail { get; set; }
        public DbSet<Company> Company { get; set; }
        public DbSet<Contact> Contact { get; set; }
        public DbSet<ContactGroup> ContactGroup { get; set; }
        public DbSet<DocumentTag> DocumentTag { get; set; }
        public DbSet<Firm> Firm { get; set; }
        public DbSet<FirmOffice> FirmOffice { get; set; }
        public DbSet<FirmOfficeAddress> FirmOfficeAddress { get; set; }
        public DbSet<NotesTag> NotesTag { get; set; }
        public DbSet<PracticeArea> PracticeArea { get; set; }
        public DbSet<RefferalSource> RefferalSource { get; set; }
        public DbSet<UserTitle> UserTitle { get; set; }
        public DbSet<UserVerification> UserVerification { get; set; }
        public DbSet<UserSubcription> UserSubcription { get; set; }
        public DbSet<Country> Country { get; set; }
        public DbSet<Lead> Lead { get; set; }
        public DbSet<HireReason> HireReason { get; set; }
        public DbSet<LeadStatus> LeadStatus { get; set; }
        public DbSet<States> States { get; set; }
        public DbSet<Tasks> Tasks { get; set; }
        public DbSet<TimeLine> TimeLine { get; set; }
        public DbSet<News> News { get; set; }
        public DbSet<FinancialDetails> FinancialDetails { get; set; }
        public DbSet<Transaction> Transaction { get; set; }
        public DbSet<ClientTransaction> ClientTransaction { get; set; }
        public DbSet<UserAgainstCase> UserAgainstCase { get; set; }
        public DbSet<RequestUsers> RequestUsers { get; set; }
        public DbSet<Notes> Notes { get; set; }
        public DbSet<Events> Events { get; set; }
        public DbSet<Message> Message { get; set; }
        public DbSet<Expense> Expenses { get; set; }
        public DbSet<PaymentTypes> PaymentTypes { get; set; }
        public DbSet<ChatGroup> ChatGroup { get; set; }
        public DbSet<GroupMessage> GroupMessage { get; set; }
        public DbSet<UserGroupMessage> UserGroupMessage { get; set; }
        public DbSet<GroupUser> GroupUser { get; set; }
        public DbSet<Decuments> Decuments { get; set; }
        public DbSet<ArchiveContact> ArchiveContact { get; set; }
        public DbSet<Faq> Faq { get; set; }
        public DbSet<ContactUs> ContactUs { get; set; }
        public DbSet<ClientLocation> ClientLocation { get; set; }
        public DbSet<CustomField> CustomField { get; set; }
        public DbSet<CFieldValue> CFieldValue { get; set; }
        public DbSet<CustomPractice> CustomPractice { get; set; }
        public DbSet<DocumentFolder> DocumentFolders { get; set; }
        public DbSet<DocSub1Folder> DocSub1Folder { get; set; }
        public DbSet<DocSub2Folder> DocSub2Folder { get; set; }
        public DbSet<DocSub3Folder> DocSub3Folder { get; set; }
        public DbSet<DocumentCategory> DocumentCategory { get; set; }
        public DbSet<TimeEntry> TimeEntry { get; set; }
        public DbSet<TestUser> TestUser { get; set; }
        public DbSet<TimeEntryActivity> TimeEntryActivity { get; set; }
        public DbSet<DocumentSign> DocumentSign { get; set; }
        public DbSet<WorkflowBase> WorkflowBase { get; set; }
        public DbSet<WorkflowAttach> WorkflowAttach { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<IdentityRole>().HasData(
            new IdentityRole { Id = "a7251255-75fc-4b74-a1a2-dcff91eb1bb2", Name = "Admin", NormalizedName = "ADMIN", ConcurrencyStamp = "7fa1a159-2c38-4205-9125-34db975e9c3b" },
            new IdentityRole { Id = "f4e833a7-e496-4448-b762-90e0630a6f91", Name = "Customer", NormalizedName = "CUSTOMER", ConcurrencyStamp = "f454d783-28c1-4ded-b54d-7403070c7b1f" },
            new IdentityRole { Id = "c0fd0c4f-1fdf-4ad0-9594-320e9dbd2b15", Name = "Attorney", NormalizedName = "ATTORNEY", ConcurrencyStamp = "035844f6-e408-418e-b51b-99f986f9a8e2" },
            new IdentityRole { Id = "f19c181c-c43d-42a2-aa90-e06bf7ac33ba", Name = "Client", NormalizedName = "CLIENT", ConcurrencyStamp = "81474c2e-36b2-4cb4-8367-3633b5b10848" },
            new IdentityRole { Id = "33ea1ca0-5973-4eac-a242-94ac321bb916", Name = "Staff", NormalizedName = "STAFF", ConcurrencyStamp = "04bc9749-050e-4a3a-8b64-684689412755" });

            modelBuilder.Entity<Service>().HasData(
            new Service { Id = 1, Name = "Unlimited Messages" },
            new Service { Id = 2, Name = "Timeline" },
            new Service { Id = 3, Name = "Calendar Events" },
            new Service { Id = 4, Name = "Reporting" },
            new Service { Id = 5, Name = "Documents Handling" },
            new Service { Id = 6, Name = "Notes Handling" });

            modelBuilder.Entity<Country>().HasData(
            new Country { Id = 1, Name = "Afghanistan", IsoCode = "AFG" },
            new Country { Id = 2, Name = "Aland Islands", IsoCode = "ALA" },
            new Country { Id = 3, Name = "Albania", IsoCode = "ALB" },
            new Country { Id = 4, Name = "Algeria", IsoCode = "DZA" },
            new Country { Id = 5, Name = "American Samoa", IsoCode = "ASM" },
            new Country { Id = 6, Name = "Andorra", IsoCode = "AND" },
            new Country { Id = 7, Name = "Angola", IsoCode = "AGO" },
            new Country { Id = 8, Name = "Anguilla", IsoCode = "AIA" },
            new Country { Id = 9, Name = "Antarctica", IsoCode = "ATA" },
            new Country { Id = 10, Name = "Antigua and Barbuda", IsoCode = "ATG" },
            new Country { Id = 11, Name = "Argentina", IsoCode = "ARG" },
            new Country { Id = 12, Name = "Armenia", IsoCode = "ARM" },
            new Country { Id = 13, Name = "Aruba", IsoCode = "ABW" },
            new Country { Id = 14, Name = "Australia", IsoCode = "AUS" },
            new Country { Id = 15, Name = "Austria", IsoCode = "AUT" },
            new Country { Id = 16, Name = "Azerbaijan", IsoCode = "AZE" },
            new Country { Id = 17, Name = "Bahamas", IsoCode = "BHS" },
            new Country { Id = 18, Name = "Bahrain", IsoCode = "BHR" },
            new Country { Id = 19, Name = "Bangladesh", IsoCode = "BGD" },
            new Country { Id = 20, Name = "Barbados", IsoCode = "BRB" },
            new Country { Id = 21, Name = "Belarus", IsoCode = "BLR" },
            new Country { Id = 22, Name = "Belgium", IsoCode = "BEL" },
            new Country { Id = 23, Name = "Belize", IsoCode = "BLZ" },
            new Country { Id = 24, Name = "Benin", IsoCode = "BEN" },
            new Country { Id = 25, Name = "Bermuda", IsoCode = "BMU" },
            new Country { Id = 26, Name = "Bhutan", IsoCode = "BTN" },
            new Country { Id = 27, Name = "Bolivia", IsoCode = "BOL" },
            new Country { Id = 28, Name = "Bosnia and Herzegovina", IsoCode = "BIH" },
            new Country { Id = 29, Name = "Botswana", IsoCode = "BWA" },
            new Country { Id = 30, Name = "Bouvet Island", IsoCode = "BVT" },
            new Country { Id = 31, Name = "Brazil", IsoCode = "BRA" },
            new Country { Id = 32, Name = "British Virgin Islands", IsoCode = "VGB" },
            new Country { Id = 33, Name = "British Indian Ocean Territory", IsoCode = "IOT" },
            new Country { Id = 34, Name = "Brunei Darussalam", IsoCode = "BRN" },
            new Country { Id = 35, Name = "Bulgaria", IsoCode = "BGR" },
            new Country { Id = 36, Name = "Burkina Faso", IsoCode = "BFA" },
            new Country { Id = 37, Name = "Burundi", IsoCode = "BDI" },
            new Country { Id = 38, Name = "Cambodia", IsoCode = "KHM" },
            new Country { Id = 39, Name = "Cameroon", IsoCode = "CMR" },
            new Country { Id = 40, Name = "Canada", IsoCode = "CAN" },
            new Country { Id = 41, Name = "Cape Verde", IsoCode = "CPV" },
            new Country { Id = 42, Name = "Cayman Islands", IsoCode = "CYM" },
            new Country { Id = 43, Name = "Central African Republic", IsoCode = "CAF" },
            new Country { Id = 44, Name = "Chad", IsoCode = "TCD" },
            new Country { Id = 45, Name = "Chile", IsoCode = "CHL" },
            new Country { Id = 46, Name = "China", IsoCode = "CHN" },
            new Country { Id = 47, Name = "Hong Kong", IsoCode = "DOM" },
            new Country { Id = 48, Name = "Macao", IsoCode = "MAC" },
            new Country { Id = 49, Name = "Christmas Island", IsoCode = "CXR" },
            new Country { Id = 50, Name = "Cocos (Keeling) Islands", IsoCode = "CCK" },
            new Country { Id = 51, Name = "Colombia", IsoCode = "COL" },
            new Country { Id = 52, Name = "Comoros", IsoCode = "COM" },
            new Country { Id = 53, Name = "Congo (Brazzaville)", IsoCode = "COG" },
            new Country { Id = 54, Name = "Congo (Kinshasa)", IsoCode = "COD" },
            new Country { Id = 55, Name = "Cook Islands", IsoCode = "COK" },
            new Country { Id = 56, Name = "Costa Rica", IsoCode = "CRI" },
            new Country { Id = 57, Name = "Cote d'Ivoire", IsoCode = "CIV" },
            new Country { Id = 58, Name = "Croatia", IsoCode = "HRV" },
            new Country { Id = 59, Name = "Cuba", IsoCode = "CUB" },
            new Country { Id = 60, Name = "Cyprus", IsoCode = "CYP" },
            new Country { Id = 61, Name = "Czech Republic", IsoCode = "CZE" },
            new Country { Id = 62, Name = "Denmark", IsoCode = "DNK" },
            new Country { Id = 63, Name = "Djibouti", IsoCode = "DJI" },
            new Country { Id = 64, Name = "Dominica", IsoCode = "DMA" },
            new Country { Id = 65, Name = "Dominican Republic", IsoCode = "DOM" },
            new Country { Id = 66, Name = "Ecuador", IsoCode = "ECU" },
            new Country { Id = 67, Name = "Egypt", IsoCode = "EGY" },
            new Country { Id = 68, Name = "Equatorial Guinea", IsoCode = "GNQ" },
            new Country { Id = 69, Name = "Eritrea", IsoCode = "ERI" },
            new Country { Id = 70, Name = "Estonia", IsoCode = "EST" },
            new Country { Id = 71, Name = "Falkland Islands", IsoCode = "FLK" },
            new Country { Id = 72, Name = "Faroe Islands", IsoCode = "FRO" },
            new Country { Id = 73, Name = "Fiji", IsoCode = "FJI" },
            new Country { Id = 74, Name = "Finland", IsoCode = "FIN" },
            new Country { Id = 75, Name = "France", IsoCode = "FRA" },
            new Country { Id = 76, Name = "French Guiana", IsoCode = "GUF" },
            new Country { Id = 77, Name = "French Polynesia", IsoCode = "PYF" },
            new Country { Id = 78, Name = "French Southern Territories", IsoCode = "ATF" },
            new Country { Id = 79, Name = "Gabon", IsoCode = "GAB" },
            new Country { Id = 80, Name = "Gambia", IsoCode = "GMB" },
            new Country { Id = 81, Name = "Georgia", IsoCode = "GHA" },
            new Country { Id = 82, Name = "Ghana", IsoCode = "GAB" },
            new Country { Id = 83, Name = "Gibraltar", IsoCode = "GIB" },
            new Country { Id = 84, Name = "Greece", IsoCode = "GRC" },
            new Country { Id = 85, Name = "Grenada", IsoCode = "GRD" },
            new Country { Id = 86, Name = "Guadeloupe", IsoCode = "GLP" },
            new Country { Id = 87, Name = "Guam", IsoCode = "GUM" },
            new Country { Id = 88, Name = "Guatemala", IsoCode = "GTM" },
            new Country { Id = 89, Name = "Guernsey", IsoCode = "GGY" },
            new Country { Id = 90, Name = "Guinea", IsoCode = "GIN" },
            new Country { Id = 91, Name = "Guinea-Bissau", IsoCode = "GNB" },
            new Country { Id = 92, Name = "Guyana", IsoCode = "GUY" },
            new Country { Id = 93, Name = "Haiti", IsoCode = "HTI" },
            new Country { Id = 94, Name = "Heard and Mcdonald Islands", IsoCode = "HMD" },
            new Country { Id = 95, Name = "Holy See (Vatican City State)", IsoCode = "VAT" },
            new Country { Id = 96, Name = "Honduras", IsoCode = "HND" },
            new Country { Id = 97, Name = "Hungary", IsoCode = "HUN" },
            new Country { Id = 98, Name = "Iceland", IsoCode = "ISL" },
            new Country { Id = 99, Name = "India", IsoCode = "IND" },
            new Country { Id = 100, Name = "Indonesia", IsoCode = "IDN" },
            new Country { Id = 101, Name = "Iran", IsoCode = "IRN" },
            new Country { Id = 102, Name = "Iraq", IsoCode = "IRQ" },
            new Country { Id = 103, Name = "Ireland", IsoCode = "IRL" },
            new Country { Id = 104, Name = "Isle of Man", IsoCode = "IMN" },
            new Country { Id = 105, Name = "Israel", IsoCode = "ISR" },
            new Country { Id = 106, Name = "Italy", IsoCode = "ITA" },
            new Country { Id = 107, Name = "Jamaica", IsoCode = "JAM" },
            new Country { Id = 108, Name = "Japan", IsoCode = "JPN" },
            new Country { Id = 109, Name = "Jersey", IsoCode = "JEY" },
            new Country { Id = 110, Name = "Jordan", IsoCode = "JOR" },
            new Country { Id = 111, Name = "Kazakhstan", IsoCode = "KAZ" },
            new Country { Id = 112, Name = "Kenya", IsoCode = "KEN" },
            new Country { Id = 113, Name = "Kiribati", IsoCode = "KIR" },
            new Country { Id = 114, Name = "Korea (North)", IsoCode = "PRK" },
            new Country { Id = 115, Name = "Korea (South)", IsoCode = "KOR" },
            new Country { Id = 116, Name = "Kuwait", IsoCode = "KWT" },
            new Country { Id = 117, Name = "Kyrgyzstan", IsoCode = "KGZ" },
            new Country { Id = 118, Name = "Lao PDR", IsoCode = "LAO" },
            new Country { Id = 119, Name = "Latvia", IsoCode = "LVA" },
            new Country { Id = 120, Name = "Lebanon", IsoCode = "LBN" },
            new Country { Id = 121, Name = "Lesotho", IsoCode = "LSO" },
            new Country { Id = 122, Name = "Liberia", IsoCode = "LBR" },
            new Country { Id = 123, Name = "Libya", IsoCode = "LBY" },
            new Country { Id = 124, Name = "Liechtenstein", IsoCode = "LIE" },
            new Country { Id = 125, Name = "Lithuania", IsoCode = "LTU" },
            new Country { Id = 126, Name = "Luxembourg", IsoCode = "LUX" },
            new Country { Id = 127, Name = "Macedonia", IsoCode = "MKD" },
            new Country { Id = 128, Name = "Madagascar", IsoCode = "MDG" },
            new Country { Id = 129, Name = "Malawi", IsoCode = "MWI" },
            new Country { Id = 130, Name = "Malaysia", IsoCode = "MDV" },
            new Country { Id = 131, Name = "Maldives", IsoCode = "MKD" },
            new Country { Id = 132, Name = "Mali", IsoCode = "MLI" },
            new Country { Id = 133, Name = "Malta", IsoCode = "MLT" },
            new Country { Id = 134, Name = "Marshall Islands", IsoCode = "MHL" },
            new Country { Id = 135, Name = "Martinique", IsoCode = "MTQ" },
            new Country { Id = 136, Name = "Mauritania", IsoCode = "MRT" },
            new Country { Id = 137, Name = "Mauritius", IsoCode = "MUS" },
            new Country { Id = 138, Name = "Mayotte", IsoCode = "MYT" },
            new Country { Id = 139, Name = "Mexico", IsoCode = "MEX" },
            new Country { Id = 140, Name = "Micronesia", IsoCode = "FSM" },
            new Country { Id = 141, Name = "Moldova", IsoCode = "MDA" },
            new Country { Id = 142, Name = "Monaco", IsoCode = "MCO" },
            new Country { Id = 143, Name = "Mongolia", IsoCode = "MNG" },
            new Country { Id = 144, Name = "Montenegro", IsoCode = "MNE" },
            new Country { Id = 145, Name = "Montserrat", IsoCode = "MSR" },
            new Country { Id = 146, Name = "Morocco", IsoCode = "MAR" },
            new Country { Id = 147, Name = "Mozambique", IsoCode = "MOZ" },
            new Country { Id = 148, Name = "Myanmar", IsoCode = "MMR" },
            new Country { Id = 149, Name = "Namibia", IsoCode = "NAM" },
            new Country { Id = 150, Name = "Nauru", IsoCode = "NRU" },
            new Country { Id = 151, Name = "Nepal", IsoCode = "NPL" },
            new Country { Id = 152, Name = "Netherlands", IsoCode = "NLD" },
            new Country { Id = 153, Name = "Netherlands Antilles", IsoCode = "ANT" },
            new Country { Id = 154, Name = "New Caledonia", IsoCode = "NCL" },
            new Country { Id = 155, Name = "New Zealand", IsoCode = "NZL" },
            new Country { Id = 156, Name = "Nicaragua", IsoCode = "NIC" },
            new Country { Id = 157, Name = "Niger", IsoCode = "NER" },
            new Country { Id = 158, Name = "Niue", IsoCode = "NIU" },
            new Country { Id = 159, Name = "Norfolk Island", IsoCode = "NFK" },
            new Country { Id = 160, Name = "Northern Mariana Islands", IsoCode = "MNP" },
            new Country { Id = 161, Name = "Norway", IsoCode = "NOR" },
            new Country { Id = 162, Name = "Oman", IsoCode = "OMN" },
            new Country { Id = 163, Name = "Pakistan", IsoCode = "PAK" },
            new Country { Id = 164, Name = "Palau", IsoCode = "PLW" },
            new Country { Id = 165, Name = "Palestinian Territory", IsoCode = "PSE" },
            new Country { Id = 166, Name = "Panama", IsoCode = "PAN" },
            new Country { Id = 167, Name = "Papua New Guinea", IsoCode = "PNG" },
            new Country { Id = 168, Name = "Paraguay", IsoCode = "PRY" },
            new Country { Id = 169, Name = "Peru", IsoCode = "PER" },
            new Country { Id = 170, Name = "Philippines", IsoCode = "PHL" },
            new Country { Id = 171, Name = "Pitcairn", IsoCode = "PCN" },
            new Country { Id = 172, Name = "Poland", IsoCode = "POL" },
            new Country { Id = 173, Name = "Portugal", IsoCode = "PRT" },
            new Country { Id = 174, Name = "Puerto Rico", IsoCode = "PRI" },
            new Country { Id = 175, Name = "Qatar", IsoCode = "QAT" },
            new Country { Id = 176, Name = "Reunion", IsoCode = "REU" },
            new Country { Id = 177, Name = "Romania", IsoCode = "ROU" },
            new Country { Id = 178, Name = "Russian Federation", IsoCode = "RUS" },
            new Country { Id = 179, Name = "Rwanda", IsoCode = "RWA" },
            new Country { Id = 180, Name = "Saint-Barthelemy", IsoCode = "BLM" },
            new Country { Id = 181, Name = "Saint Helena", IsoCode = "SHN" },
            new Country { Id = 182, Name = "Saint Kitts and Nevis", IsoCode = "KNA" },
            new Country { Id = 183, Name = "Saint Lucia", IsoCode = "LCA" },
            new Country { Id = 184, Name = "Saint-Martin", IsoCode = "MAF" },
            new Country { Id = 185, Name = "Saint Pierre and Miquelon", IsoCode = "SPM" },
            new Country { Id = 186, Name = "Saint Vincent and Grenadines", IsoCode = "VCT" },
            new Country { Id = 187, Name = "Samoa", IsoCode = "WSM" },
            new Country { Id = 188, Name = "San Marino", IsoCode = "SMR" },
            new Country { Id = 189, Name = "Sao Tome and Principe", IsoCode = "STP" },
            new Country { Id = 190, Name = "Saudi Arabia", IsoCode = "SAU" },
            new Country { Id = 191, Name = "Senegal", IsoCode = "SEN" },
            new Country { Id = 192, Name = "Serbia", IsoCode = "SRB" },
            new Country { Id = 193, Name = "Seychelles", IsoCode = "SYC" },
            new Country { Id = 194, Name = "Sierra Leone", IsoCode = "SLE" },
            new Country { Id = 195, Name = "Singapore", IsoCode = "SGP" },
            new Country { Id = 196, Name = "Slovakia", IsoCode = "SVK" },
            new Country { Id = 197, Name = "Slovenia", IsoCode = "SVN" },
            new Country { Id = 198, Name = "Solomon Islands", IsoCode = "SLB" },
            new Country { Id = 199, Name = "Somalia", IsoCode = "SOM" },
            new Country { Id = 200, Name = "South Africa", IsoCode = "ZAF" },
            new Country { Id = 201, Name = "South Georgia", IsoCode = "SGS" },
            new Country { Id = 202, Name = "South Sudan", IsoCode = "ESP" },
            new Country { Id = 203, Name = "Spain", IsoCode = "SVN" },
            new Country { Id = 204, Name = "Sri Lanka", IsoCode = "LKA" },
            new Country { Id = 205, Name = "Sudan", IsoCode = "SDN" },
            new Country { Id = 206, Name = "Suriname", IsoCode = "SUR" },
            new Country { Id = 207, Name = "Svalbard and Jan Mayen Islands", IsoCode = "SJM" },
            new Country { Id = 208, Name = "Swaziland", IsoCode = "SWZ" },
            new Country { Id = 209, Name = "Sweden", IsoCode = "SWE" },
            new Country { Id = 210, Name = "Switzerland", IsoCode = "CHE" },
            new Country { Id = 211, Name = "Syria", IsoCode = "SYR" },
            new Country { Id = 212, Name = "Taiwan", IsoCode = "TWN" },
            new Country { Id = 213, Name = "Tajikistan", IsoCode = "TJK" },
            new Country { Id = 214, Name = "Tanzania", IsoCode = "TZA" },
            new Country { Id = 215, Name = "Thailand", IsoCode = "THA" },
            new Country { Id = 216, Name = "Timor-Leste", IsoCode = "TLS" },
            new Country { Id = 217, Name = "Togo", IsoCode = "TGO" },
            new Country { Id = 218, Name = "Tokelau", IsoCode = "TKL" },
            new Country { Id = 219, Name = "Tonga", IsoCode = "TON" },
            new Country { Id = 220, Name = "Trinidad and Tobago", IsoCode = "TTO" },
            new Country { Id = 221, Name = "Tunisia", IsoCode = "TUN" },
            new Country { Id = 222, Name = "Turkey", IsoCode = "TUR" },
            new Country { Id = 223, Name = "Turkmenistan", IsoCode = "TKM" },
            new Country { Id = 224, Name = "Turks and Caicos Islands", IsoCode = "TCA" },
            new Country { Id = 225, Name = "Tuvalu", IsoCode = "TUV" },
            new Country { Id = 226, Name = "Uganda", IsoCode = "UGA" },
            new Country { Id = 227, Name = "Ukraine", IsoCode = "UKR" },
            new Country { Id = 228, Name = "United Arab Emirates", IsoCode = "ARE" },
            new Country { Id = 229, Name = "United Kingdom", IsoCode = "GBR" },
            new Country { Id = 230, Name = "United States of America", IsoCode = "USA" },
            new Country { Id = 231, Name = "US Minor Outlying Islands", IsoCode = "UMI" },
            new Country { Id = 232, Name = "Uruguay", IsoCode = "URY" },
            new Country { Id = 233, Name = "Uzbekistan", IsoCode = "UZB" },
            new Country { Id = 234, Name = "Vanuatu", IsoCode = "VUT" },
            new Country { Id = 235, Name = "Venezuela", IsoCode = "VEN" },
            new Country { Id = 236, Name = "Viet Nam", IsoCode = "VNM" },
            new Country { Id = 237, Name = "Virgin Islands", IsoCode = "VIR" },
            new Country { Id = 238, Name = "Wallis and Futuna Islands", IsoCode = "WLF" },
            new Country { Id = 239, Name = "Western Sahara", IsoCode = "ESH" },
            new Country { Id = 240, Name = "Yemen", IsoCode = "YEM" },
            new Country { Id = 241, Name = "Zambia", IsoCode = "ZMB" },
            new Country { Id = 242, Name = "Zimbabwe", IsoCode = "ZWE" }
            );

            modelBuilder.Entity<States>().HasData(
            new States { Id = 1, Name = "Alaska", Code = "AK" },
            new States { Id = 2, Name = "Alabama", Code = "AL" },
            new States { Id = 3, Name = "Arkansas", Code = "AR" },
            new States { Id = 4, Name = "Arizona", Code = "AZ" },
            new States { Id = 5, Name = "California", Code = "CO" },
            new States { Id = 6, Name = "Colorado", Code = "CA" },
            new States { Id = 7, Name = "Connecticut", Code = "CT" },
            new States { Id = 8, Name = "Washington, D.C.", Code = "DC" },
            new States { Id = 9, Name = "Delaware", Code = "DE" },
            new States { Id = 10, Name = "Florida", Code = "FL" },
            new States { Id = 11, Name = "Georgia", Code = "GA" },
            new States { Id = 12, Name = "Hawaii", Code = "HI" },
            new States { Id = 13, Name = "Iowa", Code = "IA" },
            new States { Id = 14, Name = "Idaho", Code = "ID" },
            new States { Id = 15, Name = "Illinois", Code = "IL" },
            new States { Id = 16, Name = "Indiana", Code = "IN" },
            new States { Id = 17, Name = "Kansas", Code = "KS" },
            new States { Id = 18, Name = "Kentucky", Code = "KY" },
            new States { Id = 19, Name = "Louisiana", Code = "LA" },
            new States { Id = 20, Name = "Massachusetts", Code = "MA" },
            new States { Id = 21, Name = "Maryland", Code = "MD" },
            new States { Id = 22, Name = "Maine", Code = "ME" },
            new States { Id = 23, Name = "Michigan", Code = "MI" },
            new States { Id = 24, Name = "Minnesota", Code = "MN" },
            new States { Id = 25, Name = "Missouri", Code = "MO" },
            new States { Id = 26, Name = "Mississippi", Code = "MS" },
            new States { Id = 27, Name = "Montana", Code = "MT" },
            new States { Id = 28, Name = "North Carolina", Code = "NC" },
            new States { Id = 29, Name = "North Dakota", Code = "ND" },
            new States { Id = 30, Name = "Nebraska", Code = "NE" },
            new States { Id = 31, Name = "New Hampshire", Code = "NH" },
            new States { Id = 32, Name = "New Jersey", Code = "NJ" },
            new States { Id = 33, Name = "New Mexico", Code = "NM" },
            new States { Id = 34, Name = "Nevada", Code = "AV" },
            new States { Id = 35, Name = "New York", Code = "AY" },
            new States { Id = 36, Name = "Ohio", Code = "OH" },
            new States { Id = 37, Name = "Oklahoma", Code = "OK" },
            new States { Id = 38, Name = "Oregon", Code = "OR" },
            new States { Id = 39, Name = "Pennsylvania", Code = "PA" },
            new States { Id = 40, Name = "Rhode Island", Code = "RI" },
            new States { Id = 41, Name = "South Carolina", Code = "SC" },
            new States { Id = 42, Name = "South Dakota", Code = "SD" },
            new States { Id = 43, Name = "Tennessee", Code = "TN" },
            new States { Id = 44, Name = "Texas", Code = "TX" },
            new States { Id = 45, Name = "Utah", Code = "UT" },
            new States { Id = 46, Name = "Virginia", Code = "VA" },
            new States { Id = 47, Name = "Vermont", Code = "VT" },
            new States { Id = 48, Name = "Washington", Code = "WA" },
            new States { Id = 49, Name = "Wisconsin", Code = "WI" },
            new States { Id = 50, Name = "West Virginia", Code = "WV" },
            new States { Id = 51, Name = "Wyoming", Code = "WY" },
            new States { Id = 52, Name = "U.S. Armed Forces Americas", Code = "AA" },
            new States { Id = 53, Name = "U.S. Armed Forces Europe", Code = "AE" },
            new States { Id = 54, Name = "U.S. Armed Forces Pacific", Code = "AP" },
            new States { Id = 55, Name = "American Samoa", Code = "AS" },
            new States { Id = 56, Name = "Micronesia", Code = "FM" },
            new States { Id = 57, Name = "Guam", Code = "GU" },
            new States { Id = 58, Name = "Marshall Islands", Code = "MH" },
            new States { Id = 59, Name = "Northern Mariana Islands", Code = "MP" },
            new States { Id = 60, Name = "Puerto Rico", Code = "PR" },
            new States { Id = 61, Name = "Virgin Islands", Code = "VI" }
            );
            base.OnModelCreating(modelBuilder);
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                IConfigurationRoot configuration = new ConfigurationBuilder()
                   .SetBasePath(Directory.GetCurrentDirectory())
                   .AddJsonFile("appsettings.json")
                   .Build();
                var connectionString = configuration.GetConnectionString("ConnectionString");
                optionsBuilder.UseSqlServer(connectionString);
            }
        }
    }
}
