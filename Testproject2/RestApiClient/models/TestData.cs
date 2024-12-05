namespace RestApiClient.Models
{
    public class TestData
    {
        public int SpecificPostId { get; set; }
        public int ExpectedUserIdForSpecificPost { get; set; }
        public int NonExistedPostId { get; set; }
        public int PostToPostsId { get; set; }
        public int TitleLength { get; set; }
        public int BodyLength { get; set; }
        public required UserData UserData { get; set; }
    }
}
