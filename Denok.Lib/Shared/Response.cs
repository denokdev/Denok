using System;
using System.Text.Json.Serialization;

namespace Denok.Lib.Shared 
{

    public sealed class EmptyJson {}

    public sealed class Meta
    {
        private int _page;
        private int _size;
        private int _totalData;
        private int _totalPage;
        public Meta(int page, int size, int totalData)
        {
            _page = page;
            _size = size;
            _totalData = totalData;
            CalculatePage();

        }

        [JsonPropertyName("page")]
        public int Page 
        {
            get =>  _page; 
            set => _page = value;
        }

        [JsonPropertyName("size")]
        public int Size 
        {
            get => _size; 
            set => _size = value;
        }

        [JsonPropertyName("totalData")]
        public int TotalData 
        {
            get => _totalData; 
            set => _totalData = value;
        }

        [JsonPropertyName("totalPage")]
        public int TotalPage 
        {
            get => _totalPage;
        }

        private void CalculatePage()
        {
            this._totalPage = (int) Math.Ceiling(((decimal)this._totalData) / ((decimal)this._size));
        }
    }

    public sealed class Response<T>
    {
        private bool _success;
        private int _code;
        private string _message;
        private T _data;
        private Meta _meta;

        public Response(bool success, int code, string message, T data)
        {
            _success = success;
            _code = code;
            _message = message;
            _data = data;
        }

        public Response(bool success, int code, string message, T data, Meta meta):
        this(success, code, message, data)
        {
            _meta = meta;
        }

        [JsonPropertyName("success")]
        public bool Success 
        {
            get => _success;
        }

        [JsonPropertyName("code")]
        public int Code 
        {
            get => _code;
        }

        [JsonPropertyName("message")]
        public string Message 
        {
            get => _message;
        }

        [JsonPropertyName("data")]
        public T Data 
        {
            get => _data;
        }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("meta")]
        public Meta Meta 
        {
            get => _meta;
        }
    }
}