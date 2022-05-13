#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using bancoDeImagens.Models;

namespace bancoDeImagens.Controllers
{
    public class ImagesController : Controller
    {
        private readonly Context _context;

        public ImagesController(Context context)
        {
            _context = context;
        }
        public string AddExtensao(string nomeArquivo)
        {
            string extensaoArquivo = System.IO.Path.GetExtension(nomeArquivo).ToLower();
            string[] lista = { ".gif", ".jpeg", ".jpg", ".png", ".mp4", ".mp3" };

            foreach (string extensao in lista)
                if (extensao == extensaoArquivo)
                    return extensao;
            return "none";
        }

        // GET: Images
        public async Task<IActionResult> Index()
        {
            return View(await _context.Images.ToListAsync());
        }
        public async Task<IActionResult> Galeria()
        {
            return View(await _context.Images.ToListAsync());
        }
        // GET: Images/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var image = await _context.Images
                .FirstOrDefaultAsync(m => m.Id == id);
            if (image == null)
            {
                return NotFound();
            }

            return View(image);
        }



        // GET: Images/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Images/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Size,Type,Name,Description")] Images image, IFormFile video)
        {
            var fileName = video.FileName;
            var fileSize = video.Length;
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/arq/", fileName);

            if (image.Description != null)
            {
                image.Name = fileName;
                image.Size = (int)fileSize;
                image.Type = AddExtensao(fileName);

                using (var localFile = System.IO.File.OpenWrite(filePath))
                using (var uploadedFile = video.OpenReadStream())
                {
                    uploadedFile.CopyTo(localFile);
                }

                _context.Add(image);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(image);
        }


        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var imagem = await _context.Images.FindAsync(id);
            if (imagem == null)
            {
                return NotFound();
            }
            return View(imagem);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Size,Type,Name,Description")] Images image, IFormFile video)
        {
            if (video != null)
            {
                var dataBefore = await _context.Images.FindAsync(id);
                var nameBefore = dataBefore.Name;

                var nameAfter = video.FileName;

                if (nameAfter != nameBefore)
                {
                    var nameAfter_comURL = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/arq/", nameAfter);
                    System.IO.File.Delete(nameAfter_comURL);

                    using (var localFile = System.IO.File.OpenWrite(nameAfter_comURL))
                    using (var uploadedFile = video.OpenReadStream())
                    {
                        uploadedFile.CopyTo(localFile);
                    }

                    dataBefore.Name = nameAfter;
                    dataBefore.Size = (int)video.Length;
                    dataBefore.Type = AddExtensao(nameAfter);
                    dataBefore.Description = image.Description;
                }

                _context.Update(dataBefore);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            return View(image);
        }


        // GET: Images/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var image = await _context.Images
                .FirstOrDefaultAsync(m => m.Id == id);
            if (image == null)
            {
                return NotFound();
            }

            return View(image);
        }

        // POST: Images/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var image = await _context.Images.FindAsync(id);
            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/arq/", image.Name);

            if (!System.IO.File.Exists(path))
                return RedirectToAction(nameof(Index));

            System.IO.File.Delete(path);

            _context.Images.Remove(image);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ImageExists(int id)
        {
            return _context.Images.Any(e => e.Id == id);
        }

    }
}
