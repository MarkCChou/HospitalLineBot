namespace HospitalLineBot.Models.Domain
{
    public class User
    {
        public string Name { get; set; }

        /// <summary>
        /// 輸入身分證字號
        /// </summary>
        public string Id { get; set; }

        public string Phone { get; set; }
    }
}
