namespace DemoApi.Models
{
    public class Urunler
    {
        public int UrunId { get; set; }
        public int MarkaId { get; set; }
        public int GrupId { get; set; }
        public int UreticiId { get; set; }
        public string UrunKodu { get; set; }
        public string UrunAdi { get; set; }
        public float Kdv { get; set; }
        public bool AktifMi { get; set; }
        public string? Marka {  get; set; }
    }
}
