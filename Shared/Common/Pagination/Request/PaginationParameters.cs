namespace Common.Pagination.Request
{
    public record PaginationParameters
    {
        private int? _pageSize;
        public int? PageSize
        {
            get
            {
                return _pageSize is null ? 100 : _pageSize;
            }

            set
            {
                _pageSize = value;
                if (_pageSize > 100)
                {
                    _pageSize = 100;
                }

                if (_pageSize <= 0 || _pageSize is null)
                {
                    _pageSize = 100;
                }
            }
        }

        private int? _pageNumber;
        public int? PageNumber
        {
            get
            {
                return _pageNumber is null ? 1 : _pageNumber;
            }

            set
            {
                _pageNumber = value;
                if (_pageNumber < 1 || _pageNumber is null)
                {
                    _pageNumber = 1;
                }
            }
        }

        private string _orderByColumn;
        public string OrderByColumn
        {
            get
            {
                return string.IsNullOrEmpty(_orderByColumn) ? DefaultOrderColumn.CreatedAt : _orderByColumn;
            }

            set
            {
                _orderByColumn = value;
                if (string.IsNullOrEmpty(_orderByColumn))
                {
                    _orderByColumn = DefaultOrderColumn.CreatedAt;
                }
            }
        }

        private string _sortBy;
        public string SortBy
        {
            get
            {
                return _sortBy is null || (_sortBy != SortOrder.Ascending && _sortBy != SortOrder.Descending) ? SortOrder.Ascending : _sortBy;
            }

            set
            {
                _sortBy = value;
                if (_sortBy != SortOrder.Ascending && _sortBy != SortOrder.Descending)
                {
                    _sortBy = SortOrder.Ascending;
                }
            }
        }

        public static PaginationParameters CreatePaginationParametersIfNotExists(PaginationParameters paginationParameters)
        {
            paginationParameters ??= new PaginationParameters();
            paginationParameters.PageNumber ??= default(int?);
            paginationParameters.PageSize ??= default(int?);
            paginationParameters.OrderByColumn ??= default(string);
            paginationParameters.SortBy ??= default(string);

            return paginationParameters;
        }
    }
}