using Kindergarten.Models;
using Kindergarten.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Kindergarten.Controllers
{
    public class GroupsController : Controller
    {
        private readonly ApplicationDbContext context;
		private readonly IWebHostEnvironment environment;

		public GroupsController(ApplicationDbContext context, IWebHostEnvironment environment)
        {
            this.context = context;
			this.environment = environment;
		}
        public IActionResult Index()
        {
            var groups = context.Groups.ToList();
            return View(groups);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(GroupDto groupDto)
        {
            if (groupDto.ImageFile == null)
            {
                ModelState.AddModelError("ImageFile", "Image file is required");
            }

            if (!ModelState.IsValid)
            {
                return View(groupDto);
            }

            // save image file
            string newFileName = DateTime.Now.ToString("yyyyMMddHHmmssfff");
            newFileName += Path.GetExtension(groupDto.ImageFile!.FileName);

            string imageFullPath = environment.WebRootPath + "/pictures/" + newFileName;
            using (var stream = System.IO.File.Create(imageFullPath))
            {
                groupDto.ImageFile.CopyTo(stream);
            }

            // save new group to db
            Group group = new Group()
            {
                Name = groupDto.Name,
                ChildrenCount = groupDto.ChildrenCount,
                KindergartenName = groupDto.KindergartenName,
                Teacher = groupDto.Teacher,
                ImageFileName = newFileName,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
            };

            context.Groups.Add(group);
            context.SaveChanges();

            return RedirectToAction("Index", "Groups");
        }

        public IActionResult Edit(int id)
        {
            var group = context.Groups.Find(id);

            if (group == null)
            {
                return RedirectToAction("Index", "Groups");
            }

            // create groupDto from group
            var groupDto = new GroupDto()
            {
                Name = group.Name,
                ChildrenCount = group.ChildrenCount,
                KindergartenName = group.KindergartenName,
                Teacher = group.Teacher,
            };

            ViewData["GroupId"] = group.Id;
            ViewData["ImageFileName"] = group.ImageFileName;
            ViewData["CreatedAt"] = group.CreatedAt.ToString("MM/dd/yyyy");

            return View(groupDto);
        }

        [HttpPost]
		public IActionResult Edit(int id, GroupDto groupDto)
        {
            var group = context.Groups.Find(id);

            if (group == null)
            {
                return RedirectToAction("Index", "Products");
            }

            if (!ModelState.IsValid)
            {
				ViewData["GroupId"] = group.Id;
				ViewData["ImageFileName"] = group.ImageFileName;
				ViewData["CreatedAt"] = group.CreatedAt.ToString("MM/dd/yyyy");

				return View(groupDto);
            }

            // update image if theres new
            string newFileName = group.ImageFileName;
            if (groupDto.ImageFile != null)
            {
                newFileName = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                newFileName += Path.GetExtension(groupDto.ImageFile.FileName);

                string imageFullPath = environment.WebRootPath + "/pictures/" + newFileName;
                using (var stream = System.IO.File.Create(imageFullPath))
                {
                    groupDto.ImageFile.CopyTo(stream);
                }

                // deletes old image
                string oldImageFullPath = environment.WebRootPath + "/pictures/" + group.ImageFileName;
                System.IO.File.Delete(oldImageFullPath);
            }

            // update group in db
            group.Name = groupDto.Name;
            group.KindergartenName = groupDto.KindergartenName;
            group.ChildrenCount = groupDto.ChildrenCount;
            group.Teacher = groupDto.Teacher;
            group.ImageFileName = newFileName;


            context.SaveChanges();

            return RedirectToAction("Index", "Groups");
        }

        public IActionResult Delete(int id)
        {
            var group = context.Groups.Find(id);
            if (group == null)
            {
                return RedirectToAction("Index", "Groups");
            }

            string ImageFullPath = environment.WebRootPath + "/pictures/" + group.ImageFileName;
            System.IO.File.Delete(ImageFullPath);

            context.Groups.Remove(group);
            context.SaveChanges(true);

            return RedirectToAction("Index", "Groups");
        }
	}
}
