﻿using Microsoft.Azure.Mobile.Server;
using System;
using System.Data.Entity;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.OData;

namespace MyPoetryMobileService.Models
{
    public class SimpleMappedEntityDomainManager<TData, TModel>
    : MappedEntityDomainManager<TData, TModel>
    where TData : class, Microsoft.Azure.Mobile.Server.Tables.ITableData
    where TModel : class
    {
        private Func<TModel, string> keyString;
        public SimpleMappedEntityDomainManager(DbContext context,
            HttpRequestMessage request, Func<TModel, string> keyString)
            : base(context, request)
        {
            this.keyString = keyString;
        }
        public override SingleResult<TData> Lookup(string id)
        {
            return this.LookupEntity(p => this.keyString(p) == id);
        }
        public override Task<TData> UpdateAsync(string id, Delta<TData> patch)
        {
            return this.UpdateEntityAsync(patch, id);
        }
        public override Task<bool> DeleteAsync(string id)
        {
            return this.DeleteItemAsync(id);
        }
    }
}