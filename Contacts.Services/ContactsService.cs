using Microsoft.Data.SqlClient;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.Configuration;

namespace Contacts.Services
{
    public interface IContactServices
    {
        ContactDto GetById(int? Id);
        List<ContactDto> GetList();
       // public bool Create(ContactDto contact);

        public bool Edit(ContactDto contact);


        public bool DeleteContact(int? id);
    }
    public class ContactsService : IContactServices
    {
        private readonly IConfiguration _configuration;

        public ContactsService()
        {
        }

        public ContactsService(IConfiguration Configuration)
        {
            this._configuration = Configuration;
        }
       
         public bool Edit(ContactDto contact)
         {
             string ConnectionString = _configuration.GetConnectionString("DefaultConnection");
            using (SqlConnection sqlConn = new SqlConnection(ConnectionString))
            {
                sqlConn.Open();
                SqlCommand cmd = new SqlCommand("spAddorEditContacts", sqlConn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Id", contact.Id);
                cmd.Parameters.AddWithValue("@Name", contact.FirstName);
                cmd.Parameters.AddWithValue("@Surname", contact.LastName);
                cmd.Parameters.AddWithValue("@TelephoneNumber", contact.TelephoneNumber);
                cmd.Parameters.AddWithValue("@Email", contact.Email);
                int i = cmd.ExecuteNonQuery();
                if (i >= 1)
                    return true;
                else
                    return false;
            }
            
         } 
        public bool DeleteContact(int? id)
        {
            string ConnectionString = _configuration.GetConnectionString("DefaultConnection");
            using (SqlConnection sqlConn = new SqlConnection(ConnectionString))
            {
                sqlConn.Open();
                SqlCommand cmd = new SqlCommand("spDeleteContact", sqlConn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Id", id);
                int i = cmd.ExecuteNonQuery();
                if (i >= 1)
                    return true;
                else
                    return false;
            }
        }
        public ContactDto GetById(int? Id)
        {
            string ConnectionString = _configuration.GetConnectionString("DefaultConnection");

            using (SqlConnection sqlConn = new SqlConnection(ConnectionString))
            {
                sqlConn.Open();
                SqlCommand cmd = new SqlCommand("spContactById", sqlConn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Id", Id);

                using (SqlDataReader Reader = cmd.ExecuteReader())
                {
                    if (Reader.Read())
                    {
                        var details = ToContactDto(Reader);
                        return details;
                    }
                    return null;
                    
                }

            }
        }

        public List<ContactDto> GetList()
        {
            List<ContactDto> viewContacts = new List<ContactDto>();
            string ConnectionString = _configuration.GetConnectionString("DefaultConnection");

            using (SqlConnection sqlConnection = new SqlConnection(ConnectionString))
            {
                sqlConnection.Open();
                SqlCommand cmd = new SqlCommand("spContactGetList", sqlConnection);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                using (SqlDataReader Reader = cmd.ExecuteReader())
                {

                    while (Reader.Read())
                    {
                        ContactDto details = ToContactDto(Reader);
                        viewContacts.Add(details);


                    }

                    return viewContacts;
                }
            }
        }

        private static ContactDto ToContactDto(SqlDataReader Reader)
        {
            var details = new ContactDto();
            details.Id = Reader["Id"] != DBNull.Value ? Convert.ToInt32(Reader["Id"]) : int.MinValue;
            details.FirstName = Reader["Name"] != DBNull.Value ? Convert.ToString(Reader["Name"]) : null;
            details.LastName = Reader["Surname"] != DBNull.Value ? Convert.ToString(Reader["Surname"]) : null;
            details.TelephoneNumber = Reader["TelephoneNumber"] != DBNull.Value ? Convert.ToString(Reader["TelephoneNumber"]) : null;
            details.Email = Reader["Email"] != DBNull.Value ? Convert.ToString(Reader["Email"]) : null;
            details.DateCreated = Reader["DateCreated"] != DBNull.Value ? Convert.ToDateTime(Reader["DateCreated"]) : DateTime.MinValue;
            details.DateChanged = Reader["DateChanged"] != DBNull.Value ? Convert.ToDateTime(Reader["DateChanged"]) : DateTime.MinValue;
            return details;
        }
    }
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
          CreateMap<ContactDto, ContactEditViewModel > ().ReverseMap();
        }
    }

    public class ContactDto
    {
       
        [Key]
        public int Id { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string TelephoneNumber { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public DateTime DateCreated { get; set; }
        public DateTime DateChanged { get; set; }
       

    }
   
    
    public class ContactEditViewModel //koristi za front
    {
        
        [Key]
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string TelephoneNumber { get; set; }
        public string Email { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateChanged { get; set; }
    }
   
        
}


