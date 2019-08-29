using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AzureProject.Models
{
    public class CertificatesViewModel
    {
        public int id { get; set; }
        public List<Certification> Certifications = new List<Certification>();
    }
}