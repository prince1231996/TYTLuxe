using CsvHelper.Configuration;
using Dapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Data.SqlClient;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;

namespace OleHotels.Models
{
    public interface IClientRepository
    {
        //IEnumerable<Client> GetAll();
        //Client GetById(int clientId);

        Client Login(string username, string password);

        Client Create(Client newClient);

        //bool Update(Client client);

        //bool CheckIn(int clientId);

        //bool CheckOut(int clientId);

        //void ClientCleanup(int days = 90);

    }

    public class ClientRepository : IClientRepository
    {
        private readonly IConfiguration _configuration;

        public ClientRepository(IConfiguration configuration)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        //public bool CheckIn(int clientId)
        //{
        //    var client = GetById(clientId);
        //    if (client == null)
        //    {
        //        return false;
        //    }

        //    client.IsCheckedIn = true;
        //    return Update(client);
        //}

        //public bool CheckOut(int clientId)
        //{
        //    var client = GetById(clientId);
        //    if (client == null)
        //    {
        //        return false;
        //    }

        //    client.IsCheckedIn = false;
        //    return Update(client);
        //}

        //public void ClientCleanup(int days = 90)
        //{
        //    var clients = from c in _context.Clients
        //                  join rs in _context.Reservations
        //                  on c.ClientId equals rs.ClientId
        //                  select new { c.ClientId, rs.ReservationId, rs.ReceiptId, rs.DateCheckOut };

        //    clients = clients.Where(c => (DateTime.Now - c.DateCheckOut).Days > days);
        //    foreach (var client in clients)
        //    {
        //        var addOns = _context.ReceiptAddOns.Where(ra => ra.ReceiptId == client.ReceiptId);
        //        if (addOns.Any())
        //        {
        //            _context.RemoveRange(addOns);
        //            _context.SaveChanges();
        //        }

        //        var receipt = _context.ReceiptAddOns.FirstOrDefault(r => r.ReceiptId == client.ReceiptId);
        //        if (receipt != null)
        //        {
        //            _context.Remove(receipt);
        //            _context.SaveChanges();
        //        }

        //        var reservation = _context.Reservations.FirstOrDefault(r => r.ReservationId == client.ReservationId);
        //        if (reservation != null)
        //        {
        //            _context.Remove(reservation);
        //            _context.SaveChanges();
        //        }
        //    }

        //    // Deleting distinct clients in case we have more than one reservation per client
        //    var clientIds = clients.Select(c => c.ClientId).Distinct();
        //    foreach (var id in clientIds)
        //    {
        //        var client = GetById(id);
        //        if (client != null)
        //        {
        //            _context.Remove(client);
        //            _context.SaveChanges();
        //        }

        //    }

        //}

        public Client Create(Client newClient)
        {
            //newClient.Password = newClient.Password.Encrypt();
            //_context.Clients.Add(newClient);
            //_context.SaveChanges();
            //return newClient;

            var parameters = new DynamicParameters();
            parameters.Add("@FirstName", newClient.FirstName?.Trim(), DbType.String, ParameterDirection.Input, 255);
            parameters.Add("@LastName", newClient.LastName?.Trim(), DbType.String, ParameterDirection.Input, 255);
            parameters.Add("@UserName", newClient.Username?.Trim(), DbType.String, ParameterDirection.Input, 255);
            parameters.Add("@Mobile", newClient.Phone?.Trim(), DbType.String, ParameterDirection.Input, 255);
            parameters.Add("@Email", newClient.Email?.Trim(), DbType.String, ParameterDirection.Input, 255);
            parameters.Add("@Password", newClient.Password?.Trim(), DbType.String, ParameterDirection.Input, 255);

            using (IDbConnection connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                 connection.Execute("InsertUser", parameters, commandType: CommandType.StoredProcedure);
                return newClient;

            }

        }

        //public IEnumerable<Client> GetAll()
        //{
        //    return _context.Clients;
        //}

        //public Client GetById(int clientId)
        //{
        //    return _context.Clients.FirstOrDefault(c => c.ClientId == clientId);
        //}

        public Client Login(string email, string password)
        {
            Client client = new Client();

            try
            {
                var parameters = new DynamicParameters();
                parameters.Add("@Email", email, DbType.String, ParameterDirection.Input, 255);
                parameters.Add("@Password", password, DbType.String, ParameterDirection.Input, 255);
                using (IDbConnection connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                     client = SqlMapper.Query<Client>(connection, "spGetUserForLogin", parameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
                    return client;
                }
            }
            catch (System.Exception ex)
            {

                throw ex;
            }
            //return _context.Clients.FirstOrDefault(c => c.Username == username && c.Password == password.Encrypt());
        }

        //public bool Update(Client client)
        //{
        //    var curClient = _context.Clients.FirstOrDefault(c => c.ClientId == client.ClientId);
        //    if (curClient == null)
        //    {
        //        return false;
        //    }

        //    curClient.CardId = client.CardId;
        //    curClient.Email = client.Email;
        //    curClient.FirstName = client.FirstName;
        //    curClient.LastName = client.LastName;
        //    curClient.IsCheckedIn = client.IsCheckedIn;
        //    curClient.Phone = client.Phone;
        //    _context.Clients.Update(curClient);

        //    return _context.SaveChanges() > 0;
        //}
    }
}