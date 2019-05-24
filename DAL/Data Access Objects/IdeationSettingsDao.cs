namespace DAL.Data_Access_Objects
{
    /*
     * @author Niels Van Zandbergen
     */
    public class IdeationSettingsDao
    {
        public int ModuleId { get; set; }
        public bool Field { get; set; }
        public bool ClosedField { get; set; }
        public bool MapField { get; set; }
        public bool VideoField { get; set; }
        public bool ImageField { get; set; }
    }
}