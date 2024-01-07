using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace server.Domain.Migrations
{
    /// <inheritdoc />
    public partial class AddNewTableCategoriesQuestionsImages : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "categories_questions",
                columns: table =>
                    new
                    {
                        category_question_id = table
                            .Column<int>(type: "INTEGER", nullable: false)
                            .Annotation("Sqlite:Autoincrement", true),
                        title = table.Column<string>(type: "TEXT", nullable: true)
                    },
                constraints: table =>
                {
                    table.PrimaryKey("PK_categories_questions", x => x.category_question_id);
                }
            );

            migrationBuilder.CreateTable(
                name: "roles",
                columns: table =>
                    new
                    {
                        role_id = table
                            .Column<int>(type: "INTEGER", nullable: false)
                            .Annotation("Sqlite:Autoincrement", true),
                        name = table.Column<string>(type: "TEXT", nullable: false)
                    },
                constraints: table =>
                {
                    table.PrimaryKey("PK_roles", x => x.role_id);
                }
            );

            migrationBuilder.CreateTable(
                name: "categories_questions_images",
                columns: table =>
                    new
                    {
                        category_question_image_id = table
                            .Column<int>(type: "INTEGER", nullable: false)
                            .Annotation("Sqlite:Autoincrement", true),
                        path = table.Column<string>(type: "TEXT", nullable: false),
                        category_question_id = table.Column<int>(type: "INTEGER", nullable: false)
                    },
                constraints: table =>
                {
                    table.PrimaryKey(
                        "PK_categories_questions_images",
                        x => x.category_question_image_id
                    );
                    table.ForeignKey(
                        name: "FK_categories_questions_images_categories_questions_category_question_id",
                        column: x => x.category_question_id,
                        principalTable: "categories_questions",
                        principalColumn: "category_question_id",
                        onDelete: ReferentialAction.Cascade
                    );
                }
            );

            migrationBuilder.CreateTable(
                name: "questions",
                columns: table =>
                    new
                    {
                        question_id = table
                            .Column<int>(type: "INTEGER", nullable: false)
                            .Annotation("Sqlite:Autoincrement", true),
                        content = table.Column<string>(type: "TEXT", nullable: true),
                        category_question_id = table.Column<int>(type: "INTEGER", nullable: false)
                    },
                constraints: table =>
                {
                    table.PrimaryKey("PK_questions", x => x.question_id);
                    table.ForeignKey(
                        name: "FK_questions_categories_questions_category_question_id",
                        column: x => x.category_question_id,
                        principalTable: "categories_questions",
                        principalColumn: "category_question_id",
                        onDelete: ReferentialAction.Cascade
                    );
                }
            );

            migrationBuilder.CreateTable(
                name: "users",
                columns: table =>
                    new
                    {
                        user_id = table
                            .Column<int>(type: "INTEGER", nullable: false)
                            .Annotation("Sqlite:Autoincrement", true),
                        login = table.Column<string>(type: "TEXT", nullable: true),
                        password = table.Column<string>(type: "TEXT", nullable: true),
                        role_id = table.Column<int>(type: "INTEGER", nullable: false),
                        refresh_token = table.Column<string>(type: "TEXT", nullable: true),
                        refresh_token_expiry_time = table.Column<DateTime>(
                            type: "TEXT",
                            nullable: true
                        )
                    },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.user_id);
                    table.ForeignKey(
                        name: "FK_users_roles_role_id",
                        column: x => x.role_id,
                        principalTable: "roles",
                        principalColumn: "role_id",
                        onDelete: ReferentialAction.Cascade
                    );
                }
            );

            migrationBuilder.CreateTable(
                name: "answers",
                columns: table =>
                    new
                    {
                        answer_id = table
                            .Column<int>(type: "INTEGER", nullable: false)
                            .Annotation("Sqlite:Autoincrement", true),
                        content = table.Column<string>(type: "TEXT", nullable: true),
                        is_true = table.Column<bool>(type: "INTEGER", nullable: false),
                        question_id = table.Column<int>(type: "INTEGER", nullable: false)
                    },
                constraints: table =>
                {
                    table.PrimaryKey("PK_answers", x => x.answer_id);
                    table.ForeignKey(
                        name: "FK_answers_questions_question_id",
                        column: x => x.question_id,
                        principalTable: "questions",
                        principalColumn: "question_id",
                        onDelete: ReferentialAction.Cascade
                    );
                }
            );

            migrationBuilder.CreateTable(
                name: "questions_images",
                columns: table =>
                    new
                    {
                        image_id = table
                            .Column<int>(type: "INTEGER", nullable: false)
                            .Annotation("Sqlite:Autoincrement", true),
                        path = table.Column<string>(type: "TEXT", nullable: true),
                        question_id = table.Column<int>(type: "INTEGER", nullable: false)
                    },
                constraints: table =>
                {
                    table.PrimaryKey("PK_questions_images", x => x.image_id);
                    table.ForeignKey(
                        name: "FK_questions_images_questions_question_id",
                        column: x => x.question_id,
                        principalTable: "questions",
                        principalColumn: "question_id",
                        onDelete: ReferentialAction.Cascade
                    );
                }
            );

            migrationBuilder.CreateTable(
                name: "users_questions",
                columns: table =>
                    new
                    {
                        user_question_id = table
                            .Column<int>(type: "INTEGER", nullable: false)
                            .Annotation("Sqlite:Autoincrement", true),
                        user_id = table.Column<int>(type: "INTEGER", nullable: false),
                        question_id = table.Column<int>(type: "INTEGER", nullable: false),
                        complete = table.Column<bool>(type: "INTEGER", nullable: false),
                        answer_number = table.Column<int>(type: "INTEGER", nullable: false),
                        user_question_expiry_time = table.Column<DateTime>(
                            type: "TEXT",
                            nullable: false
                        )
                    },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users_questions", x => x.user_question_id);
                    table.ForeignKey(
                        name: "FK_users_questions_questions_question_id",
                        column: x => x.question_id,
                        principalTable: "questions",
                        principalColumn: "question_id",
                        onDelete: ReferentialAction.Cascade
                    );
                    table.ForeignKey(
                        name: "FK_users_questions_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Cascade
                    );
                }
            );

            migrationBuilder.CreateIndex(
                name: "IX_answers_question_id",
                table: "answers",
                column: "question_id"
            );

            migrationBuilder.CreateIndex(
                name: "IX_categories_questions_images_category_question_id",
                table: "categories_questions_images",
                column: "category_question_id",
                unique: true
            );

            migrationBuilder.CreateIndex(
                name: "IX_questions_category_question_id",
                table: "questions",
                column: "category_question_id"
            );

            migrationBuilder.CreateIndex(
                name: "IX_questions_images_question_id",
                table: "questions_images",
                column: "question_id",
                unique: true
            );

            migrationBuilder.CreateIndex(name: "IX_roles_name", table: "roles", column: "name");

            migrationBuilder.CreateIndex(
                name: "IX_users_login",
                table: "users",
                column: "login",
                unique: true
            );

            migrationBuilder.CreateIndex(
                name: "IX_users_role_id",
                table: "users",
                column: "role_id"
            );

            migrationBuilder.CreateIndex(
                name: "IX_users_questions_question_id",
                table: "users_questions",
                column: "question_id"
            );

            migrationBuilder.CreateIndex(
                name: "IX_users_questions_user_id",
                table: "users_questions",
                column: "user_id"
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(name: "answers");

            migrationBuilder.DropTable(name: "categories_questions_images");

            migrationBuilder.DropTable(name: "questions_images");

            migrationBuilder.DropTable(name: "users_questions");

            migrationBuilder.DropTable(name: "questions");

            migrationBuilder.DropTable(name: "users");

            migrationBuilder.DropTable(name: "categories_questions");

            migrationBuilder.DropTable(name: "roles");
        }
    }
}
