using System;
using System.Collections.Generic;
using Blog.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Blog.Data.Mappings
{
    public class PostMap : IEntityTypeConfiguration<Post>
    {
        public void Configure(EntityTypeBuilder<Post> builder)
        {
            // Tabela
            builder.ToTable("Post");

            // Chave Primaria
            builder.HasKey(x => x.Id);

            // Identity
            builder.Property(x => x.Id)
                .ValueGeneratedOnAdd()
                .UseIdentityColumn();

            // Propriedades
            builder.Property(x => x.LastUpdateDate)
                .IsRequired()
                .HasColumnName("LastUpdateDate")
                .HasColumnType("SMALLDATETIME")
                .HasMaxLength(60)
                .HasDefaultValueSql("GETDATE()"); // Uma função que roda no banco
                                                  //.HasDefaultValue(DateTime.Now.ToUniversalTime());

            // Indices
            builder
                .HasIndex(x => x.Slug, "IX_Post_Slug")
                .IsUnique();

            // Relacionamentos (one-to-many - um para muitos)
            builder.HasOne(x => x.Author) // Um para muitos
                .WithMany(x => x.Posts)
                .HasConstraintName("FK_Post_Author") //Define o nome do FK
                .OnDelete(DeleteBehavior.Cascade); // Define o que vai acontecer ao deleter um post

            builder.HasOne(x => x.Category)
               .WithMany(x => x.Posts)
               .HasConstraintName("FK_Post_Category")
               .OnDelete(DeleteBehavior.Cascade);

            // Relacionamentos (many-to-many - muitos para muitos)
            // Com esse mapemento o EF irá criar uma tabela virtual baseado em um objeto
            // Poderiamos também criar uma nova classe para passar nesse caso.
            builder.HasMany(x => x.Tags)
                .WithMany(x => x.Posts)
                .UsingEntity<Dictionary<string, object>>(
                    "PostTag", // Nome da tabela (nome da classe)
                    post => post.HasOne<Tag>() // Colunas (propriedades)
                    .WithMany()
                    .HasForeignKey("PostId")
                    .HasConstraintName("FK_PostTag_PostId")
                    .OnDelete(DeleteBehavior.Cascade),
                    tag => tag.HasOne<Post>()
                        .WithMany()
                        .HasForeignKey("TagId")
                        .HasConstraintName("FK_PostTag_TagId")
                        .OnDelete(DeleteBehavior.Cascade));
        }
    }
}