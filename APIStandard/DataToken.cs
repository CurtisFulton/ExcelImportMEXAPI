﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Services.Client;
using System.Linq;
using System.Text;

namespace MEXModel
{
    public class DataToken : MEXEntities
    {
        public DataToken(Uri serviceRoot, string username = "admin", string password = "admin") : base(serviceRoot)
        {
            this.ReadingEntity += DataToken_ReadingEntity;

            this.BuildingRequest += (sender, e) => {
                string auth = Convert.ToBase64String(
                    Encoding.UTF8.GetBytes($"{username}:{password}")
                    );

                e.Headers.Add("Authorization", $"Basic {auth}");
            };
        }
        
        private void DataToken_ReadingEntity(object sender, System.Data.Services.Client.ReadingWritingEntityEventArgs e)
        {
            var entity = (e.Entity as INotifyPropertyChanged);
            entity.PropertyChanged += Entity_PropertyChanged;
        }

        private void Entity_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            this.UpdateObject(sender);
        }

        //public bool DynamicUpdateObject<T>(T entity)
        //{
        //    // To be tested
        //    try {
        //        var entityType = entity.GetType();
        //        var tableName = entityType.ToString();
        //        var properties = entity.GetType().GetProperties();
        //        var primaryKey = properties[0];

        //        var setString = "";

        //        for (int i = 1; i < properties.Length; i++)
        //        {
        //            if (i == properties.Length - 1)
        //                setString += $"{properties[i].Name} = {properties[i].GetValue(entity, null)}";
        //            else
        //                setString += $"{properties[i].Name} = {properties[i].GetValue(entity, null)}, ";
        //        }


        //        var query = $"'Update {tableName} set ({setString} where {tableName}ID = {primaryKey.GetValue(entity, null)}";
        //        DataServiceQuery<string> dapperQuery = this.CreateQuery<string>("DapperQuery").AddQueryOption("sql", ValidateDapperQuery(query));

        //        return true;
        //    } catch (Exception e) {
        //        Console.WriteLine(e);
        //        return false;
        //    }
        //}

        /// <summary>
        /// Helper method for writing Dapper Queries. 
        /// </summary>
        /// <typeparam name="T">Type of object the query returns</typeparam>
        /// <param name="query">SQL Query</param>
        /// <returns>List of objects returned by the query</returns>
        public List<T> DynamicQuery<T>(string query)
        {
            DataServiceQuery<string> dapperQuery = this.CreateQuery<string>("DapperQuery").AddQueryOption("sql", ValidateDapperQuery(query));
            List<T> results = null;

            try {
            } catch (Exception e) {
                Console.WriteLine(e);
            }

            return results;

        }

        /// <summary>
        /// Validates the Query to make sure it has single quotation marks around it.
        /// </summary>
        /// <param name="query">Dapper Query</param>
        /// <returns>Validated Query</returns>
        private string ValidateDapperQuery(string query)
        {
            // Possibly add other validation here in the future
            if (query[0] != '\'')
                query = "'" + query;

            if (query.Last() != '\'')
                query += "'";

            return query;
        }
    }
}
