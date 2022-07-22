using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using APIPractice.Data;
using APIPractice.Models;
using System.Data;
using System.Windows;
using System.IO;
using static System.Net.WebRequestMethods;
using Microsoft.AspNetCore.Http;
using YamlDotNet.RepresentationModel;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;
using System.Text;

namespace APIPractice.Controllers
{
    public class MoviesController : Controller
    {
        private readonly APIPracticeContext _context;

        public MoviesController(APIPracticeContext context)
        {
            _context = context;
        }

        // GET: Movies
        public async Task<IActionResult> Index(string searchString)
        {
            // Movieテーブルから全てのデータを取得するLINQクエリ
            var movies = _context.Movie.Select(m => m);
            // タイトル検索処理
            if (!string.IsNullOrEmpty(searchString))
            {
                // タイトルに検索文字列が含まれるデータを抽出するLINQクエリ
                movies = movies.Where(s => s.AddDate == DateTime.Parse(searchString));
            }
            // ToListAsyncメソッドが呼び出されたらクエリが実行され(遅延実行)、その結果をビューに返す
            return View(await movies.ToListAsync());
        }

        // GET: Movies/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Movie == null)
            {
                return NotFound();
            }

            var movie = await _context.Movie
                .FirstOrDefaultAsync(m => m.Id == id);
            if (movie == null)
            {
                return NotFound();
            }

            return View(movie);
        }

        // GET: Movies/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Movies/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Adress,Text,AddDate")] Movie movie)
        {
            if (ModelState.IsValid)
            {
                _context.Add(movie);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(movie);
        }

        // GET: Movies/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Movie == null)
            {
                return NotFound();
            }

            var movie = await _context.Movie.FindAsync(id);
            if (movie == null)
            {
                return NotFound();
            }
            return View(movie);
        }

        // POST: Movies/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Adress,Text,AddDate")] Movie movie)
        {
            if (id != movie.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(movie);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MovieExists(movie.Id))
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
            return View(movie);
        }

        // GET: Movies/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Movie == null)
            {
                return NotFound();
            }

            var movie = await _context.Movie
                .FirstOrDefaultAsync(m => m.Id == id);
            if (movie == null)
            {
                return NotFound();
            }

            return View(movie);
        }

        // POST: Movies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Movie == null)
            {
                return Problem("Entity set 'APIPracticeContext.Movie'  is null.");
            }
            var movie = await _context.Movie.FindAsync(id);
            if (movie != null)
            {
                _context.Movie.Remove(movie);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MovieExists(int id)
        {
            return (_context.Movie?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        public record DocumentCreateRequest([FromForm(Name = "userfile")]IFormFile File);

        [HttpPost, ActionName("UploadFile")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UploadFile([FromForm] DocumentCreateRequest request)
        {
            //型をStreamReaderにする
             var sr = new StreamReader(request.File.OpenReadStream(), Encoding.UTF8);
            
            // デシリアライザインスタンス作成
            var deserializer = new Deserializer();

            // yamlデータのオブジェクトを作成
            Movie deserializeObject = deserializer.Deserialize<Movie>(sr);

            //変更できるか
            if (ModelState.IsValid)
            {
                try
                {
                    //表に追加
                    _context.Add(deserializeObject);
                    //Insert処理実行
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MovieExists(deserializeObject.Id))
                    {
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            
            return RedirectToAction(nameof(Index));
        }
    }
}
