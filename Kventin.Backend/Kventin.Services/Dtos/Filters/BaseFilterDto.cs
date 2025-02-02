namespace Kventin.Services.Dtos.Filters
{
    public class BaseFilterDto
    {
        public BaseFilterDto() 
        {
            Take = 10;
            Page = 1;
        }

        private int _take;
        public int Take 
        { 
            get
            {
                if (_take < 0)
                    return 10;

                return _take > 50 ? 50 : _take; 
            }
            set => _take = value;
        }

        private int _page;
        public int Page
        {
            get => _page < 1 ? 1 : _page;
            set => _page = value;
        }

        public int Skip => (Page - 1) * Take;
    }
}
