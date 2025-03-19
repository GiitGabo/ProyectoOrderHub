namespace JarredsOrderHub.Models
{
    public class FooterInfoViewModel
    {
        public string FooterAbout { get; set; }
        public string FooterTelefono { get; set; }
        public string FooterEmail { get; set; }
        public string FooterDireccion { get; set; }
        public string FooterFAQ { get; set; }
        public string FooterTérminos { get; set; }

        public Contacto Contacto { get; set; }
        public TerminosCondiciones Terminos { get; set; }
        public List<Preguntas> Preguntas { get; set; }
        public AcercaDe AcercaDe { get; set; }
    }

}
