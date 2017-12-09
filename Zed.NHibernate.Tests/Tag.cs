using System;
using Zed.Domain;
using Zed.Utilities;

namespace Zed.NHibernate.Tests.Model {
    public class Tag : Entity {

        #region Fields and Properties

        /// <summary>
        /// Tag name
        /// </summary>
        private string name;

        /// <summary>
        /// Gets or Sets tag name
        /// </summary>
        public virtual string Name {
            get { return name; }
            set {
                if (String.IsNullOrWhiteSpace(value)) throw new ArgumentNullException("value", "Tag name must contain some value.");

                name = value;
            }
        }


        /// <summary>
        /// Tag slug
        /// </summary>
        private string slug;

        /// <summary>
        /// Gets or Sets tag slug
        /// </summary>
        public virtual string Slug { get { return slug; } }

        /// <summary>
        /// Base tag
        /// </summary>
        private readonly Tag baseTag;

        /// <summary>
        /// Gets base tag.
        /// </summary>
        public virtual Tag BaseTag { get { return baseTag; } }

        #endregion

        #region Constructors and Init

        /// <summary>
        /// Default constructor that creates a new instance of Tag class.
        /// </summary>
        protected Tag() { }

        /// <summary>
        /// Creates instance of Tag class with provided tag name and slug
        /// </summary>
        /// <param name="name">Tag name</param>
        /// <param name="slug">Tag slug</param>
        private Tag(string name, string slug) {
            Name = name;
            SetSlug(slug);

            baseTag = null;
        }

        /// <summary>
        /// Creates instance of Tag class with provided tag name, slug and base tag
        /// </summary>
        /// <param name="name">Tag name</param>
        /// <param name="slug">Tag slug</param>
        /// <param name="baseTag">Basetag</param>
        internal Tag(string name, string slug, Tag baseTag) : this(name, slug) {
            this.baseTag = baseTag;
        }

        /// <summary>
        /// Creates instance of Tag class with provided tag name and base tag
        /// </summary>
        /// <param name="name">Tag name</param>
        /// <param name="baseTag">Basetag</param>
        internal Tag(string name, Tag baseTag) : this(name, name, baseTag) { }

        #endregion

        #region Methods

        /// <summary>
        /// Sets provided argument to slug. If necessary provided argument is transformed
        /// to slug form.
        /// </summary>
        /// <param name="slugValue">Slug value</param>
        public virtual void SetSlug(string slugValue) {
            if (String.IsNullOrWhiteSpace(slugValue)) throw new ArgumentNullException("slugValue", "Tag slugValue must contain some value.");

            slug = slugValue.ToSlug();
        }

        /// <summary>
        /// Creates instance of Tag class with provided tag name on English language.
        /// Slug is automatically generated based on tag name.
        /// </summary>
        /// <param name="name">Tag name on English.</param>
        public static Tag CreateBaseTag(string name) {
            return new Tag(name, name);
        }

        /// <summary>
        /// Creates instance of Tag class with provided tag name and slug on English.
        /// </summary>
        /// <param name="name">Tag name on English</param>
        /// <param name="slug">Tag slug</param>
        public static Tag CreateBaseTag(string name, string slug) {
            return new Tag(name, slug);
        }

        #endregion

    }
}
