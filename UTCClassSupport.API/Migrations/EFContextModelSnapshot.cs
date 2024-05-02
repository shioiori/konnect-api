﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using UTCClassSupport.API.Infrustructure.Data;

#nullable disable

namespace UTCClassSupport.API.Migrations
{
    [DbContext(typeof(EFContext))]
    partial class EFContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.24")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("ChatUser", b =>
                {
                    b.Property<string>("ChatsId")
                        .HasColumnType("varchar(95)");

                    b.Property<string>("JoinnersId")
                        .HasColumnType("varchar(95)");

                    b.HasKey("ChatsId", "JoinnersId");

                    b.HasIndex("JoinnersId");

                    b.ToTable("ChatUser");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("ClaimType")
                        .HasColumnType("longtext");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("longtext");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("varchar(95)");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("RoleClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("ClaimType")
                        .HasColumnType("longtext");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("longtext");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("varchar(95)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("UserClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("varchar(95)");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("varchar(95)");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("longtext");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("varchar(95)");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("UserLogins", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("varchar(95)");

                    b.Property<string>("RoleId")
                        .HasColumnType("varchar(95)");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("UserRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("varchar(95)");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("varchar(95)");

                    b.Property<string>("Name")
                        .HasColumnType("varchar(95)");

                    b.Property<string>("Value")
                        .HasColumnType("longtext");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("UserTokens", (string)null);
                });

            modelBuilder.Entity("PermissionRole", b =>
                {
                    b.Property<string>("PermissionsId")
                        .HasColumnType("varchar(95)");

                    b.Property<string>("RolesId")
                        .HasColumnType("varchar(95)");

                    b.HasKey("PermissionsId", "RolesId");

                    b.HasIndex("RolesId");

                    b.ToTable("PermissionRole");
                });

            modelBuilder.Entity("UTCClassSupport.API.Models.Bulletin", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("varchar(95)")
                        .HasColumnName("id");

                    b.Property<int>("Approved")
                        .HasColumnType("int")
                        .HasColumnName("approved");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("longtext")
                        .HasColumnName("content");

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasColumnType("longtext")
                        .HasColumnName("created_by");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime")
                        .HasColumnName("created_date");

                    b.Property<string>("GroupId")
                        .IsRequired()
                        .HasColumnType("varchar(95)")
                        .HasColumnName("group_id");

                    b.Property<string>("LastModifiedBy")
                        .HasColumnType("longtext")
                        .HasColumnName("last_modified_by");

                    b.Property<DateTime?>("LastModifiedDate")
                        .HasColumnType("datetime")
                        .HasColumnName("last_modified_date");

                    b.HasKey("Id");

                    b.HasIndex("GroupId");

                    b.ToTable("bulletins");
                });

            modelBuilder.Entity("UTCClassSupport.API.Models.Chat", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("varchar(95)")
                        .HasColumnName("id");

                    b.Property<string>("Avatar")
                        .HasColumnType("longtext")
                        .HasColumnName("content");

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasColumnType("longtext")
                        .HasColumnName("created_by");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime")
                        .HasColumnName("created_date");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext")
                        .HasColumnName("name");

                    b.HasKey("Id");

                    b.ToTable("chats");
                });

            modelBuilder.Entity("UTCClassSupport.API.Models.Comment", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("varchar(95)")
                        .HasColumnName("id");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("longtext")
                        .HasColumnName("content");

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasColumnType("longtext")
                        .HasColumnName("created_by");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime")
                        .HasColumnName("created_date");

                    b.Property<string>("PostId")
                        .IsRequired()
                        .HasColumnType("varchar(95)")
                        .HasColumnName("post_id");

                    b.HasKey("Id");

                    b.HasIndex("PostId");

                    b.ToTable("comments");
                });

            modelBuilder.Entity("UTCClassSupport.API.Models.Group", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("varchar(95)")
                        .HasColumnName("id");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext")
                        .HasColumnName("name");

                    b.HasKey("Id");

                    b.ToTable("groups");
                });

            modelBuilder.Entity("UTCClassSupport.API.Models.Message", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("varchar(95)")
                        .HasColumnName("id");

                    b.Property<string>("ChatId")
                        .IsRequired()
                        .HasColumnType("varchar(95)")
                        .HasColumnName("chat_id");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("longtext")
                        .HasColumnName("content");

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasColumnType("longtext")
                        .HasColumnName("created_by");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime")
                        .HasColumnName("created_date");

                    b.Property<string>("FileId")
                        .HasColumnType("varchar(95)")
                        .HasColumnName("file_id");

                    b.HasKey("Id");

                    b.HasIndex("ChatId");

                    b.HasIndex("FileId");

                    b.ToTable("messages");
                });

            modelBuilder.Entity("UTCClassSupport.API.Models.MessageFile", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("varchar(95)")
                        .HasColumnName("id");

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasColumnType("longtext")
                        .HasColumnName("created_by");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime")
                        .HasColumnName("created_date");

                    b.Property<string>("Url")
                        .IsRequired()
                        .HasColumnType("longtext")
                        .HasColumnName("url");

                    b.HasKey("Id");

                    b.ToTable("message_files");
                });

            modelBuilder.Entity("UTCClassSupport.API.Models.Permission", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("varchar(95)");

                    b.Property<int>("Name")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("permissions");
                });

            modelBuilder.Entity("UTCClassSupport.API.Models.Role", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("varchar(95)");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("longtext");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("varchar(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("varchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex");

                    b.ToTable("Roles", (string)null);
                });

            modelBuilder.Entity("UTCClassSupport.API.Models.Schedule", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("varchar(95)")
                        .HasColumnName("id");

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasColumnType("longtext")
                        .HasColumnName("created_by");

                    b.Property<int>("SendBeforeMinutes")
                        .HasColumnType("int")
                        .HasColumnName("send_before_minutes");

                    b.Property<int>("ShiftId")
                        .HasColumnType("int")
                        .HasColumnName("shift_id");

                    b.HasKey("Id");

                    b.HasIndex("ShiftId")
                        .IsUnique();

                    b.ToTable("schedules");
                });

            modelBuilder.Entity("UTCClassSupport.API.Models.ShareFile", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("varchar(95)")
                        .HasColumnName("id");

                    b.Property<bool>("Approved")
                        .HasColumnType("tinyint(1)")
                        .HasColumnName("approved");

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasColumnType("longtext")
                        .HasColumnName("created_by");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime")
                        .HasColumnName("created_date");

                    b.Property<string>("FolderId")
                        .IsRequired()
                        .HasColumnType("varchar(95)")
                        .HasColumnName("folder_id");

                    b.Property<string>("Url")
                        .IsRequired()
                        .HasColumnType("longtext")
                        .HasColumnName("url");

                    b.HasKey("Id");

                    b.HasIndex("FolderId");

                    b.ToTable("share_files");
                });

            modelBuilder.Entity("UTCClassSupport.API.Models.ShareFolder", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("varchar(95)")
                        .HasColumnName("id");

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasColumnType("longtext")
                        .HasColumnName("created_by");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime")
                        .HasColumnName("created_date");

                    b.Property<string>("GroupId")
                        .IsRequired()
                        .HasColumnType("varchar(95)")
                        .HasColumnName("group_id");

                    b.Property<string>("Url")
                        .IsRequired()
                        .HasColumnType("longtext")
                        .HasColumnName("url");

                    b.HasKey("Id");

                    b.HasIndex("GroupId");

                    b.ToTable("share_folders");
                });

            modelBuilder.Entity("UTCClassSupport.API.Models.Shift", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    b.Property<int>("Code")
                        .HasColumnType("int")
                        .HasColumnName("code");

                    b.Property<int>("Credit")
                        .HasColumnType("int")
                        .HasColumnName("credit");

                    b.Property<int>("Day")
                        .HasColumnType("int")
                        .HasColumnName("day");

                    b.Property<DateTime>("From")
                        .HasColumnType("datetime")
                        .HasColumnName("from");

                    b.Property<string>("Location")
                        .IsRequired()
                        .HasColumnType("longtext")
                        .HasColumnName("location");

                    b.Property<string>("Subject")
                        .IsRequired()
                        .HasColumnType("longtext")
                        .HasColumnName("subject");

                    b.Property<string>("SubjectClass")
                        .IsRequired()
                        .HasColumnType("longtext")
                        .HasColumnName("subject_class");

                    b.Property<string>("SubjectCode")
                        .IsRequired()
                        .HasColumnType("longtext")
                        .HasColumnName("subject_code");

                    b.Property<string>("TimetableId")
                        .IsRequired()
                        .HasColumnType("varchar(95)")
                        .HasColumnName("timetable_id");

                    b.Property<DateTime>("To")
                        .HasColumnType("datetime")
                        .HasColumnName("to");

                    b.HasKey("Id");

                    b.HasIndex("TimetableId");

                    b.ToTable("shifts");
                });

            modelBuilder.Entity("UTCClassSupport.API.Models.Timetable", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("varchar(95)")
                        .HasColumnName("id");

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasColumnType("longtext")
                        .HasColumnName("created_by");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime")
                        .HasColumnName("created_date");

                    b.Property<string>("GroupId")
                        .IsRequired()
                        .HasColumnType("longtext")
                        .HasColumnName("group_id");

                    b.Property<bool>("IsSynchronize")
                        .HasColumnType("tinyint(1)")
                        .HasColumnName("is_synchronize");

                    b.Property<int>("Remind")
                        .HasColumnType("int")
                        .HasColumnName("remind_time");

                    b.Property<string>("Url")
                        .IsRequired()
                        .HasColumnType("longtext")
                        .HasColumnName("url");

                    b.HasKey("Id");

                    b.ToTable("timetables");
                });

            modelBuilder.Entity("UTCClassSupport.API.Models.User", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("varchar(95)");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("longtext");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("varchar(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("tinyint(1)");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetime");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("varchar(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("varchar(256)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("longtext");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("longtext");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("longtext");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("varchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex");

                    b.ToTable("Users", (string)null);
                });

            modelBuilder.Entity("UTCClassSupport.API.Models.UserGroupRole", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("varchar(95)")
                        .HasColumnName("user_id");

                    b.Property<string>("GroupId")
                        .HasColumnType("varchar(95)")
                        .HasColumnName("group_id");

                    b.Property<string>("RoleId")
                        .HasColumnType("varchar(95)")
                        .HasColumnName("role_id");

                    b.HasKey("UserId", "GroupId", "RoleId");

                    b.HasIndex("GroupId");

                    b.HasIndex("RoleId");

                    b.ToTable("users_groups_roles");
                });

            modelBuilder.Entity("ChatUser", b =>
                {
                    b.HasOne("UTCClassSupport.API.Models.Chat", null)
                        .WithMany()
                        .HasForeignKey("ChatsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("UTCClassSupport.API.Models.User", null)
                        .WithMany()
                        .HasForeignKey("JoinnersId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("UTCClassSupport.API.Models.Role", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("UTCClassSupport.API.Models.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("UTCClassSupport.API.Models.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("UTCClassSupport.API.Models.Role", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("UTCClassSupport.API.Models.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("UTCClassSupport.API.Models.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("PermissionRole", b =>
                {
                    b.HasOne("UTCClassSupport.API.Models.Permission", null)
                        .WithMany()
                        .HasForeignKey("PermissionsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("UTCClassSupport.API.Models.Role", null)
                        .WithMany()
                        .HasForeignKey("RolesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("UTCClassSupport.API.Models.Bulletin", b =>
                {
                    b.HasOne("UTCClassSupport.API.Models.Group", "Group")
                        .WithMany("Bulletins")
                        .HasForeignKey("GroupId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Group");
                });

            modelBuilder.Entity("UTCClassSupport.API.Models.Comment", b =>
                {
                    b.HasOne("UTCClassSupport.API.Models.Bulletin", "Post")
                        .WithMany("Comments")
                        .HasForeignKey("PostId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Post");
                });

            modelBuilder.Entity("UTCClassSupport.API.Models.Message", b =>
                {
                    b.HasOne("UTCClassSupport.API.Models.Chat", "Chat")
                        .WithMany("Messages")
                        .HasForeignKey("ChatId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("UTCClassSupport.API.Models.MessageFile", "File")
                        .WithMany()
                        .HasForeignKey("FileId");

                    b.Navigation("Chat");

                    b.Navigation("File");
                });

            modelBuilder.Entity("UTCClassSupport.API.Models.Schedule", b =>
                {
                    b.HasOne("UTCClassSupport.API.Models.Shift", "Shift")
                        .WithOne("Schedule")
                        .HasForeignKey("UTCClassSupport.API.Models.Schedule", "ShiftId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Shift");
                });

            modelBuilder.Entity("UTCClassSupport.API.Models.ShareFile", b =>
                {
                    b.HasOne("UTCClassSupport.API.Models.ShareFolder", "Folder")
                        .WithMany("Files")
                        .HasForeignKey("FolderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Folder");
                });

            modelBuilder.Entity("UTCClassSupport.API.Models.ShareFolder", b =>
                {
                    b.HasOne("UTCClassSupport.API.Models.Group", "Group")
                        .WithMany("Folders")
                        .HasForeignKey("GroupId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Group");
                });

            modelBuilder.Entity("UTCClassSupport.API.Models.Shift", b =>
                {
                    b.HasOne("UTCClassSupport.API.Models.Timetable", "Timetable")
                        .WithMany("Shifts")
                        .HasForeignKey("TimetableId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Timetable");
                });

            modelBuilder.Entity("UTCClassSupport.API.Models.UserGroupRole", b =>
                {
                    b.HasOne("UTCClassSupport.API.Models.Group", "Group")
                        .WithMany()
                        .HasForeignKey("GroupId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("UTCClassSupport.API.Models.Role", "Role")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("UTCClassSupport.API.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Group");

                    b.Navigation("Role");

                    b.Navigation("User");
                });

            modelBuilder.Entity("UTCClassSupport.API.Models.Bulletin", b =>
                {
                    b.Navigation("Comments");
                });

            modelBuilder.Entity("UTCClassSupport.API.Models.Chat", b =>
                {
                    b.Navigation("Messages");
                });

            modelBuilder.Entity("UTCClassSupport.API.Models.Group", b =>
                {
                    b.Navigation("Bulletins");

                    b.Navigation("Folders");
                });

            modelBuilder.Entity("UTCClassSupport.API.Models.ShareFolder", b =>
                {
                    b.Navigation("Files");
                });

            modelBuilder.Entity("UTCClassSupport.API.Models.Shift", b =>
                {
                    b.Navigation("Schedule");
                });

            modelBuilder.Entity("UTCClassSupport.API.Models.Timetable", b =>
                {
                    b.Navigation("Shifts");
                });
#pragma warning restore 612, 618
        }
    }
}
