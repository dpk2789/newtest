using Accounts.Model.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TreeUtility;

namespace Accounts.Model.Model
{
    public class ItemCategory : ITreeNode<ItemCategory>
    {
        public int Id { get; set; }

        private int? _parentCategoryId;

        [Display(Name = "Parent Category")]
        public int? ParentCategoryId
        {
            get { return _parentCategoryId; }
            set
            {
                if (Id == value)
                    throw new InvalidOperationException("A category cannot have itself as its parent.");

                _parentCategoryId = value;
            }
        }
        [Required(ErrorMessage = "You must enter a category name.")]
        [StringLength(20, ErrorMessage = "Category names must be 20 characters or shorter.")]
        [Display(Name = "Category")]
        public string CategoryName { get; set; }

        public virtual ItemCategory Parent { get; set; }
        public IList<ItemCategory> Children { get; set; }
        public virtual IList<Item> Items { get; set; }

    }
}
