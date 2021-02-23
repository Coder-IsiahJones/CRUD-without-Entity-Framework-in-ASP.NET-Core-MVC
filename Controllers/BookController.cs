using CRUD_without_Entity_Framework_in_ASP.NET_Core_MVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Data;

namespace CRUD_without_Entity_Framework_in_ASP.NET_Core_MVC.Controllers
{
    public class BookController : Controller
    {
        private IConfiguration _configuration;

        public BookController(IConfiguration configuration)
        {
            this._configuration = configuration;
        }

        // GET: Book
        public IActionResult Index()
        {
            DataTable dtbl = new DataTable();
            using (SqlConnection sqlConnection = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
            {
                sqlConnection.Open();
                SqlDataAdapter sqlData = new SqlDataAdapter("BookViewAll", sqlConnection);
                sqlData.SelectCommand.CommandType = CommandType.StoredProcedure;
                sqlData.Fill(dtbl);
            }

            return View(dtbl);
        }

        // GET: Book/AddOrEdit/
        public IActionResult AddOrEdit(int? id)
        {
            BookVM bookViewModel = new BookVM();

            if (id > 0)
            {
                bookViewModel = FetchBookById(id);
            }

            return View(bookViewModel);
        }

        // POST: Book/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddOrEdit(int id, [Bind("BookId,Title,Author,Price")] BookVM bookVM)
        {
            if (ModelState.IsValid)
            {
                using (SqlConnection sqlConnection = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {
                    sqlConnection.Open();
                    SqlCommand sqlcmd = new SqlCommand("BookAddOrEdit", sqlConnection);
                    sqlcmd.CommandType = CommandType.StoredProcedure;
                    sqlcmd.Parameters.AddWithValue("BookId", bookVM.BookId);
                    sqlcmd.Parameters.AddWithValue("Title", bookVM.Title);
                    sqlcmd.Parameters.AddWithValue("Author", bookVM.Author);
                    sqlcmd.Parameters.AddWithValue("Price", bookVM.Price);
                    sqlcmd.ExecuteNonQuery();
                }
                return RedirectToAction(nameof(Index));
            }

            return View();
        }

        // GET: Book/Delete/5
        public IActionResult Delete(int? id)
        {
            BookVM bookVM = FetchBookById(id);

            return View(bookVM);
        }

        // POST: Book/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            using (SqlConnection sqlConnection = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
            {
                sqlConnection.Open();
                SqlCommand sqlcmd = new SqlCommand("BookDeleteById", sqlConnection);
                sqlcmd.CommandType = CommandType.StoredProcedure;
                sqlcmd.Parameters.AddWithValue("BookId", id);
                sqlcmd.ExecuteNonQuery();
            }

            return RedirectToAction(nameof(Index));
        }

        [NonAction]
        public BookVM FetchBookById(int? id)
        {
            BookVM bookVM = new BookVM();

            using (SqlConnection sqlConnection = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
            {
                DataTable dtbl = new DataTable();
                sqlConnection.Open();
                SqlDataAdapter sqlData = new SqlDataAdapter("BookViewById", sqlConnection);
                sqlData.SelectCommand.CommandType = CommandType.StoredProcedure;
                sqlData.SelectCommand.Parameters.AddWithValue("BookId", id);
                sqlData.Fill(dtbl);

                if (dtbl.Rows.Count == 1)
                {
                    bookVM.BookId = Convert.ToInt32(dtbl.Rows[0]["BookId"].ToString());
                    bookVM.Title = dtbl.Rows[0]["Title"].ToString();
                    bookVM.Author = dtbl.Rows[0]["Author"].ToString();
                    bookVM.Price = Convert.ToDecimal(dtbl.Rows[0]["Price"].ToString());
                }
                return bookVM;
            }
        }
    }
}