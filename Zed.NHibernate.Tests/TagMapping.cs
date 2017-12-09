using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;

namespace Zed.NHibernate.Tests.Model {
    class TagMapping : ClassMapping<Tag> {
        public TagMapping() {
            Table("Tags");

            Id(x => x.Id, m => m.Generator(Generators.Native));
            Property(x => x.Name,
                m => {
                    m.Access(Accessor.NoSetter);
                    m.NotNullable(true);
                });
            Property(x => x.Slug,
                m => {
                    m.Access(Accessor.NoSetter);
                    m.NotNullable(true);
                });
            ManyToOne(x => x.BaseTag,
                m => {
                    m.Column("BaseTagId");
                    m.Access(Accessor.NoSetter);
                    m.Cascade(Cascade.Persist);
                });
        }
    }
}
