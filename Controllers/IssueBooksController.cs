using LibraryManagmentSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LibraryManagmentSystem.Controllers
{
    public class IssueBooksController : Controller
    {
        private LibraryManagmentSystemEntities db = new LibraryManagmentSystemEntities();
        int MemberID;
        // GET: IssueBooks
        public ActionResult Index(int? id)
        {
            IssuebooksViewModel issuebooksViewModel = new IssuebooksViewModel();
            issuebooksViewModel.MemberId = (int)id;
            PageLoad((int)id);           

            return View(issuebooksViewModel);
        }
        public ActionResult AddBookToStudentAccount(IssuebooksViewModel issuebooksViewModelid)
        {
            try
            {
                int strDDLValue = Convert.ToInt32(Request.Form["ObjList"]);
                Book book = new Book();
                LendedBook lendedBook = new LendedBook();
                MemberID = issuebooksViewModelid.MemberId;
                book = db.Books.Where(x => x.BookId == strDDLValue).FirstOrDefault();
                var books = db.LendedBooks.Where(x => x.MemberId == MemberID).ToList();
                bool IsExist = false;
                if (books.Count < 5)
                {
                    foreach (var item in books)
                    {
                        if (item.BookId == strDDLValue)
                        {
                            IsExist = true;
                            break;
                        }
                    }
                    if (!IsExist)
                    {
                        if (book != null)
                        {
                            if (book.BooksCount > 0)
                            {
                                lendedBook.BookId = strDDLValue;
                                lendedBook.MemberId = MemberID;
                                db.LendedBooks.Add(lendedBook);

                                book.BooksCount = book.BooksCount - 1;
                                db.SaveChanges();
                            } 
                        }
                    }
                }
                PageLoad(MemberID);
                return View("index", issuebooksViewModelid);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ActionResult RemoveBooks(int id,int MemId)
        {
            LendedBook lendedBook = new LendedBook();
            Book book = new Book();
            book = db.Books.Where(x => x.BookId == id).FirstOrDefault();
            IssuebooksViewModel issuebooksViewModel = new IssuebooksViewModel();
            issuebooksViewModel.MemberId = MemId;
            lendedBook = db.LendedBooks.Where(x => x.BookId == id && x.MemberId == MemId).FirstOrDefault();
            if (lendedBook != null)
            {
                db.LendedBooks.Remove(lendedBook);
                book.BooksCount = book.BooksCount + 1;
                db.SaveChanges();
            }
            
            PageLoad(MemId);
            return View("index", issuebooksViewModel);
        }
        public void PageLoad(int id)
        {
            var books = db.Books.Where(x => x.BooksCount > 0).ToList();
            List<SelectListItem> ObjList = new List<SelectListItem>();
            foreach (var item in books)
            {
                ObjList.Add(new SelectListItem { Text = item.BookName, Value = item.BookId.ToString() });
            }

            ViewBag.ListBook = ObjList;

            Member member = new Member();
            member = db.Members.Where(x => x.MemberId == id).FirstOrDefault();
            ViewBag.MemberName = member.LastName + ", " + member.FirstName;
            ViewBag.MemberType = member.MemberType;
            ViewBag.MemberID = id;

            var LendingDetails = db.LendedBooks.Where(x => x.MemberId == id).ToList();
            List<string> BookName = new List<string>();
            foreach (var item in LendingDetails)
            {
                var Book = db.Books.Where(x => x.BookId == item.BookId).FirstOrDefault();
                var BookDetails = Book.BookId + "," + Book.BookName;
                BookName.Add(BookDetails);
            }
            ViewBag.BooksIssuedToStudent = BookName;
        }
    }
}