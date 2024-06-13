// Models/UrunDto.cs
namespace DemoApi.Models
{
    public class UrunDto
    {
        public int UrunId { get; set; }
        public string UrunAdi { get; set; }
        public string UrunKodu { get; set; }
        public decimal Kdv { get; set; }
        public bool AktifMi { get; set; }
        public string Marka { get; set; } // Marka ismi
    }
}
