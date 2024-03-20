namespace Entities.DataTransferObjects
{
	/*[Serializable]*/ //Serileştirilebilir, örneğin xml formatında maplemede 500 hatası dönüyordu çözüm olarak bu attribute gerekli
	//public record BookDto(int Id, String Title, decimal Price); //Çıktı backfieldlarla geldiği için aşağıdaki tarzda yazdık
	public record BookDto
	{
        public int Id { get; set; }
        public String Title { get; set; }
        public decimal Price { get; set; }
    }
	
}
