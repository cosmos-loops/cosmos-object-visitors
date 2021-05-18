namespace CosmosObjectVisitorUT.Model
{
    public class NiceAct4
    {
        public string Name { get; set; }

        public int Age { get; set; }
        
        public NiceAct4B AddressModel { get; set; }
    }

    public class NiceAct4B
    {
        public string Address { get; set; }
        
        public NiceAct4C Country { get; set; }
    }

    public class NiceAct4C
    {
        public string City { get; set; }
    }
}