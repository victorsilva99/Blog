using Blog.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Blog.Data.Mappings
{
    public class CategoryMap : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            // Nome da tabela
            builder.ToTable("Category");

            // Chave Primária
            builder.HasKey(x => x.Id);

            // Identity
            builder.Property(x => x.Id)
                .ValueGeneratedOnAdd()
                .UseIdentityColumn(); // PrimaryKey (1,1 )

            // Propriedades
            builder.Property(x => x.Name)
                .IsRequired() // Not null
                .HasColumnName("Name") // Nome da coluna
                .HasColumnType("NVARCHAR") // Tipo da coluna
                .HasMaxLength(80); // Quantidade de caracteres

            builder.Property(x => x.Slug)
                .IsRequired()
                .HasColumnName("Slug")
                .HasColumnType("VARCHAR")
                .HasMaxLength(80);

            // Indices
            builder.HasIndex(x => x.Slug, "IX_Category_Slug") // (coluna, nome_indice)
                .IsUnique(); // Unique
            
        }
    }
}