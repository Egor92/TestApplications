namespace SendArchives.Models
{
    public class UserData
    {
        public string Login { get; private set; }
        public string UserPassword { get; private set; }
        public string Email { get; private set; }
        public string ArchivePassword { get; private set; }

        public static Result<UserData> Create(string data)
        {
            var items = data.Split('\t');
            if (items.Length != 4)
            {
                return new Result<UserData>
                {
                    IsSuccess = false,
                    ErrorMessage = $"Line must have 4 columns. But it has {items.Length} columns",
                };
            }

            var userData = new UserData
            {
                Login = items[0],
                UserPassword = items[1],
                Email = items[2],
                ArchivePassword = items[3],
            };
            return new Result<UserData>
            {
                IsSuccess = true,
                Data = userData,
            };
        }
    }
}