#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FinalProgram_9.Data;
using FinalProgram_9.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace FinalProgram_9.Controllers
{
    [Authorize]
    public class QuestionsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private UserManager<ApplicationUser> _userManager;

        public QuestionsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Questions

        public async Task<IActionResult> Index(string OrderingQ) 
        {
            if (OrderingQ != null)
            {
                if (OrderingQ == "Active")
                {
                    var mostActiveQuestions = _context.Questions.Include(q => q.User).Include(q => q.Answers).OrderByDescending(q => q.Answers.Count).ToList();
                    return View(mostActiveQuestions);
                }
                else
                {
                    var mostActiveQuestions = _context.Questions.Include(q => q.User).Include(q => q.Answers).OrderByDescending(q => q.DateOfCreate).ToList();
                    return View(mostActiveQuestions);
                }
            }
            var applicationDbContext = _context.Questions.Include(q => q.User);
            return View(await applicationDbContext.ToListAsync());
        }

        //Add page function

        [AllowAnonymous]
        public async Task<IActionResult> testPage(

                  int? pageNumber)
        {

            var question = _context.Questions;
            int pageSize = 10;
            return View(await PaginatedList<Question>.CreateAsync(question.AsNoTracking(), pageNumber ?? 1, pageSize));
        }




        // GET: Questions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var question = await _context.Questions.Include(q=>q.Answers).ThenInclude(a=>a.Comments).Include(q=>q.Comments)
                .Include(q => q.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (question == null)
            {
                return NotFound();
            }

            return View(question);
        }

        [HttpPost]
        public IActionResult MarkAsCorrect(int questionId, int answerId)
        {
            
            Answer correctAnswer = _context.Answers.Include(a => a.Question).ThenInclude(q => q.User).First(a => a.Id == answerId);
            correctAnswer.IsCorrect = true;
            _context.SaveChanges();
            return RedirectToAction("Details", new { questionId = questionId });
        }

        // GET: Questions/Create
        public IActionResult Create()
        {
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id");
            SelectList tagList = new SelectList(_context.Tags, "Id", "Name");
            ViewBag.TagList = tagList;
            return View();
        }

        // POST: Questions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Body,DateOfCreate,DateOfClose,UpVote,DownVote,UserId")] Question question, int tagId)
        {
            if (ModelState.IsValid)
            {
                _context.Add(question);
                QuestionTag qt = new QuestionTag(question.Id, tagId);
                question.QuestionTags.Add(qt);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", question.UserId);
         
  
           
            return View(question);
        }
        


        // GET: Questions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var question = await _context.Questions.FindAsync(id);
            if (question == null)
            {
                return NotFound();
            }
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", question.UserId);
            return View(question);
        }

        // POST: Questions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Body,DateOfCreate,DateOfClose,UpVote,DownVote,UserId")] Question question)
        {
            if (id != question.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(question);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!QuestionExists(question.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", question.UserId);
            return View(question);
        }

        
        // GET: Questions/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var question = await _context.Questions
                .Include(q => q.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (question == null)
            {
                return NotFound();
            }

            return View(question);
        }


        [HttpPost]

        //one create post is better,one controller method for Posthttp page , per page per method
        public async Task<IActionResult>CreateAnswer(string AnswerContent, int questionId)
        {
            var UserName = User.Identity.Name;
            var LogedUser = _context.Users.First(u => u.UserName == UserName);
            var UserId = LogedUser.Id;
       

            //USER MANAGERER ASYNC 

            // pagination,admin

            var newAnswer = new Answer();
            newAnswer.AnswerContent=AnswerContent;
            newAnswer.QuestionId=questionId;
            _context.Answers.Add(newAnswer);
            _context.SaveChanges();
            return RedirectToAction("Details",new { id = questionId });

        }


        // POST: Questions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var question = await _context.Questions.FindAsync(id);
            _context.Questions.Remove(question);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        private bool QuestionExists(int id)
        {
            return _context.Questions.Any(e => e.Id == id);
        }
    }
}
