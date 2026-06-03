using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MyCRM.Common;
using MyCRM.Models;
using System.Security.Claims;
using System.Web;

namespace MyCRM.DataBase
{
    public partial class ApplicationDbcontext : DbContext
    {

        public ApplicationDbcontext()
        {
        }

        public ApplicationDbcontext(DbContextOptions<ApplicationDbcontext> options)
            : base(options)
        {
        }

        /* public DbSet<ShopCategory> shopCategories { get; set; }
         public DbSet<User> user { get; set; }*/
        public virtual DbSet<Efmigrationshistory> Efmigrationshistories { get; set; } = null!;
        public virtual DbSet<TActivity> TActivities { get; set; } = null!;
        public virtual DbSet<TActivityRemark> TActivityRemarks { get; set; } = null!;
        public virtual DbSet<TClue> TClues { get; set; } = null!;
        public virtual DbSet<TClueRemark> TClueRemarks { get; set; } = null!;
        public virtual DbSet<TCustomer> TCustomers { get; set; } = null!;
        public virtual DbSet<TCustomerRemark> TCustomerRemarks { get; set; } = null!;
        public virtual DbSet<TDicType> TDicTypes { get; set; } = null!;
        public virtual DbSet<TDicValue> TDicValues { get; set; } = null!;
        public virtual DbSet<TPermission> TPermissions { get; set; } = null!;
        public virtual DbSet<TProduct> TProducts { get; set; } = null!;
        public virtual DbSet<TRole> TRoles { get; set; } = null!;
        public virtual DbSet<TRolePermission> TRolePermissions { get; set; } = null!;
        public virtual DbSet<TSystemInfo> TSystemInfos { get; set; } = null!;
        public virtual DbSet<TTran> TTrans { get; set; } = null!;
        public virtual DbSet<TTranHistory> TTranHistories { get; set; } = null!;
        public virtual DbSet<TTranRemark> TTranRemarks { get; set; } = null!;
        public virtual DbSet<TUser> TUsers { get; set; } = null!;
        public virtual DbSet<TUserRole> TUserRoles { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            /*if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseMySql("server=Localhost;database=dlyk;user=root;password=Lqb@902002", Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.32-mysql"));
            }*/
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);

            modelBuilder.UseCollation("utf8mb4_0900_ai_ci")
                .HasCharSet("utf8mb4");

            modelBuilder.Entity<Efmigrationshistory>(entity =>
            {
                entity.HasKey(e => e.MigrationId)
                    .HasName("PRIMARY");

                entity.ToTable("__efmigrationshistory");

                entity.Property(e => e.MigrationId).HasMaxLength(150);

                entity.Property(e => e.ProductVersion).HasMaxLength(32);
            });

            modelBuilder.Entity<TActivity>(entity =>
            {
                entity.ToTable("t_activity");

                entity.HasComment("市场活动表")
                    .HasCharSet("utf8mb3")
                    .UseCollation("utf8mb3_general_ci");

                entity.HasIndex(e => e.CreateBy, "create_by");

                entity.HasIndex(e => e.EditBy, "edit_by");

                entity.HasIndex(e => e.OwnerId, "owner");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasComment("主键，自动增长，活动ID");

                entity.Property(e => e.Cost)
                    .HasPrecision(11, 2)
                    .HasColumnName("cost")
                    .HasComment("活动预算");

                entity.Property(e => e.CreateBy)
                    .HasColumnName("create_by")
                    .HasComment("活动创建人");

                entity.Property(e => e.CreateTime)
                    .HasColumnType("datetime")
                    .HasColumnName("create_time")
                    .HasComment("活动创建时间");

                entity.Property(e => e.Description)
                    .HasMaxLength(255)
                    .HasColumnName("description")
                    .HasComment("活动描述");

                entity.Property(e => e.EditBy)
                    .HasColumnName("edit_by")
                    .HasComment("活动编辑人");

                entity.Property(e => e.EditTime)
                    .HasColumnType("datetime")
                    .HasColumnName("edit_time")
                    .HasComment("活动编辑时间");

                entity.Property(e => e.EndTime)
                    .HasColumnType("datetime")
                    .HasColumnName("end_time")
                    .HasComment("活动结束时间");

                entity.Property(e => e.Name)
                    .HasMaxLength(128)
                    .HasColumnName("name")
                    .HasComment("活动名称");

                entity.Property(e => e.OwnerId)
                    .HasColumnName("owner_id")
                    .HasComment("活动所属人ID");

                entity.Property(e => e.StartTime)
                    .HasColumnType("datetime")
                    .HasColumnName("start_time")
                    .HasComment("活动开始时间");

                entity.HasOne(d => d.CreateByNavigation)
                    .WithMany(p => p.TActivityCreateByNavigations)
                    .HasForeignKey(d => d.CreateBy)
                    .HasConstraintName("t_activity_ibfk_2");

                entity.HasOne(d => d.EditByNavigation)
                    .WithMany(p => p.TActivityEditByNavigations)
                    .HasForeignKey(d => d.EditBy)
                    .HasConstraintName("t_activity_ibfk_3");

                entity.HasOne(d => d.Owner)
                    .WithMany(p => p.TActivityOwners)
                    .HasForeignKey(d => d.OwnerId)
                    .HasConstraintName("t_activity_ibfk_1");
                
            });


          

            modelBuilder.Entity<TActivityRemark>(entity =>
            {
                entity.ToTable("t_activity_remark");

                entity.HasComment("市场活动备注表")
                    .HasCharSet("utf8mb3")
                    .UseCollation("utf8mb3_general_ci");

                entity.HasIndex(e => e.ActivityId, "activity_id");

                entity.HasIndex(e => e.CreateBy, "t_activity_remark_ibfk_2");

                entity.HasIndex(e => e.EditBy, "t_activity_remark_ibfk_3");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasComment("主键，自动增长，活动备注ID");

                entity.Property(e => e.ActivityId)
                    .HasColumnName("activity_id")
                    .HasComment("活动ID");

                entity.Property(e => e.CreateBy)
                    .HasColumnName("create_by")
                    .HasComment("备注创建人");

                entity.Property(e => e.CreateTime)
                    .HasColumnType("datetime")
                    .HasColumnName("create_time")
                    .HasComment("备注创建时间");

                entity.Property(e => e.Deleted)
                    .HasColumnName("deleted")
                    .HasComment("删除状态（0正常，1删除）");

                entity.Property(e => e.EditBy)
                    .HasColumnName("edit_by")
                    .HasComment("备注编辑人");

                entity.Property(e => e.EditTime)
                    .HasColumnType("datetime")
                    .HasColumnName("edit_time")
                    .HasComment("备注编辑时间");

                entity.Property(e => e.NoteContent)
                    .HasMaxLength(255)
                    .HasColumnName("note_content")
                    .HasComment("备注内容");

                entity.HasOne(d => d.Activity)
                    .WithMany(p => p.TActivityRemarks)
                    .HasForeignKey(d => d.ActivityId)
                    .HasConstraintName("t_activity_remark_ibfk_1");

                entity.HasOne(d => d.CreateByNavigation)
                    .WithMany(p => p.TActivityRemarkCreateByNavigations)
                    .HasForeignKey(d => d.CreateBy)
                    .HasConstraintName("t_activity_remark_ibfk_2");

                entity.HasOne(d => d.EditByNavigation)
                    .WithMany(p => p.TActivityRemarkEditByNavigations)
                    .HasForeignKey(d => d.EditBy)
                    .HasConstraintName("t_activity_remark_ibfk_3");

                entity.HasQueryFilter(c => c.Deleted == 0);
            });

            modelBuilder.Entity<TClue>(entity =>
            {
                entity.ToTable("t_clue");

                entity.HasComment("线索表")
                    .HasCharSet("utf8mb3")
                    .UseCollation("utf8mb3_general_ci");

                entity.HasIndex(e => e.Appellation, "appellation");

                entity.HasIndex(e => e.CreateBy, "create_by");

                entity.HasIndex(e => e.EditBy, "edit_by");

                entity.HasIndex(e => e.OwnerId, "owner");

                entity.HasIndex(e => e.Source, "source");

                entity.HasIndex(e => e.State, "state");

                entity.HasIndex(e => e.IntentionProduct, "t_clue_ibfk_10");

                entity.HasIndex(e => e.ActivityId, "t_clue_ibfk_7");

                entity.HasIndex(e => e.NeedLoan, "t_clue_ibfk_8");

                entity.HasIndex(e => e.IntentionState, "t_clue_ibfk_9");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasComment("主键，自动增长，线索ID");

                entity.Property(e => e.ActivityId)
                    .HasColumnName("activity_id")
                    .HasComment("活动ID");

                entity.Property(e => e.Address)
                    .HasMaxLength(128)
                    .HasColumnName("address")
                    .HasComment("地址");

                entity.Property(e => e.Age)
                    .HasColumnName("age")
                    .HasComment("年龄");

                entity.Property(e => e.Appellation)
                    .HasColumnName("appellation")
                    .HasComment("称呼");

                entity.Property(e => e.CreateBy)
                    .HasColumnName("create_by")
                    .HasComment("创建人");

                entity.Property(e => e.CreateTime)
                    .HasColumnType("datetime")
                    .HasColumnName("create_time")
                    .HasComment("创建时间");

                entity.Property(e => e.Description)
                    .HasMaxLength(255)
                    .HasColumnName("description")
                    .HasComment("线索描述");

                entity.Property(e => e.EditBy)
                    .HasColumnName("edit_by")
                    .HasComment("编辑人");

                entity.Property(e => e.EditTime)
                    .HasColumnType("datetime")
                    .HasColumnName("edit_time")
                    .HasComment("编辑时间");

                entity.Property(e => e.Email)
                    .HasMaxLength(128)
                    .HasColumnName("email")
                    .HasComment("邮箱");

                entity.Property(e => e.FullName)
                    .HasMaxLength(64)
                    .HasColumnName("full_name")
                    .HasComment("姓名");

                entity.Property(e => e.IntentionProduct)
                    .HasColumnName("intention_product")
                    .HasComment("意向产品");

                entity.Property(e => e.IntentionState)
                    .HasColumnName("intention_state")
                    .HasComment("意向状态");

                entity.Property(e => e.Job)
                    .HasMaxLength(64)
                    .HasColumnName("job")
                    .HasComment("职业");

                entity.Property(e => e.NeedLoan)
                    .HasColumnName("need_loan")
                    .HasComment("是否需要贷款（0不需要，1需要）");

                entity.Property(e => e.NextContactTime)
                    .HasColumnType("datetime")
                    .HasColumnName("next_contact_time")
                    .HasComment("下次联系时间");

                entity.Property(e => e.OwnerId)
                    .HasColumnName("owner_id")
                    .HasComment("线索所属人ID");

                entity.Property(e => e.Phone)
                    .HasMaxLength(18)
                    .HasColumnName("phone")
                    .HasComment("手机号");

                entity.Property(e => e.Qq)
                    .HasMaxLength(20)
                    .HasColumnName("qq")
                    .HasComment("QQ号");

                entity.Property(e => e.Source)
                    .HasColumnName("source")
                    .HasComment("线索来源");

                entity.Property(e => e.State)
                    .HasColumnName("state")
                    .HasComment("线索状态");

                entity.Property(e => e.Weixin)
                    .HasMaxLength(128)
                    .HasColumnName("weixin")
                    .HasComment("微信号");

                entity.Property(e => e.YearIncome)
                    .HasPrecision(10, 2)
                    .HasColumnName("year_income")
                    .HasComment("年收入");

                entity.HasOne(d => d.Activity)
                    .WithMany(p => p.TClues)
                    .HasForeignKey(d => d.ActivityId)
                    .HasConstraintName("t_clue_ibfk_7");

                entity.HasOne(d => d.AppellationNavigation)
                    .WithMany(p => p.TClueAppellationNavigations)
                    .HasForeignKey(d => d.Appellation)
                    .HasConstraintName("t_clue_ibfk_1");

                entity.HasOne(d => d.CreateByNavigation)
                    .WithMany(p => p.TClueCreateByNavigations)
                    .HasForeignKey(d => d.CreateBy)
                    .HasConstraintName("t_clue_ibfk_5");

                entity.HasOne(d => d.EditByNavigation)
                    .WithMany(p => p.TClueEditByNavigations)
                    .HasForeignKey(d => d.EditBy)
                    .HasConstraintName("t_clue_ibfk_6");

                entity.HasOne(d => d.IntentionProductNavigation)
                    .WithMany(p => p.TClues)
                    .HasForeignKey(d => d.IntentionProduct)
                    .HasConstraintName("t_clue_ibfk_10");

                entity.HasOne(d => d.IntentionStateNavigation)
                    .WithMany(p => p.TClueIntentionStateNavigations)
                    .HasForeignKey(d => d.IntentionState)
                    .HasConstraintName("t_clue_ibfk_9");

                entity.HasOne(d => d.NeedLoanNavigation)
                    .WithMany(p => p.TClueNeedLoanNavigations)
                    .HasForeignKey(d => d.NeedLoan)
                    .HasConstraintName("t_clue_ibfk_8");

                entity.HasOne(d => d.Owner)
                    .WithMany(p => p.TClueOwners)
                    .HasForeignKey(d => d.OwnerId)
                    .HasConstraintName("t_clue_ibfk_4");

                entity.HasOne(d => d.SourceNavigation)
                    .WithMany(p => p.TClueSourceNavigations)
                    .HasForeignKey(d => d.Source)
                    .HasConstraintName("t_clue_ibfk_3");

                entity.HasOne(d => d.StateNavigation)
                    .WithMany(p => p.TClueStateNavigations)
                    .HasForeignKey(d => d.State)
                    .HasConstraintName("t_clue_ibfk_2");
            });

            modelBuilder.Entity<TClueRemark>(entity =>
            {
                entity.ToTable("t_clue_remark");

                entity.HasComment("线索跟踪记录表")
                    .HasCharSet("utf8mb3")
                    .UseCollation("utf8mb3_general_ci");

                entity.HasIndex(e => e.ClueId, "clue_id");

                entity.HasIndex(e => e.CreateBy, "create_by");

                entity.HasIndex(e => e.EditBy, "edit_by");

                entity.HasIndex(e => e.NoteWay, "t_clue_remark_ibfk_4");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasComment("主键，自动增长，线索备注ID");

                entity.Property(e => e.ClueId)
                    .HasColumnName("clue_id")
                    .HasComment("线索ID");

                entity.Property(e => e.CreateBy)
                    .HasColumnName("create_by")
                    .HasComment("跟踪人");

                entity.Property(e => e.CreateTime)
                    .HasColumnType("datetime")
                    .HasColumnName("create_time")
                    .HasComment("跟踪时间");

                entity.Property(e => e.Deleted)
                    .HasColumnName("deleted")
                    .HasComment("删除状态（0正常，1删除）");

                entity.Property(e => e.EditBy)
                    .HasColumnName("edit_by")
                    .HasComment("编辑人");

                entity.Property(e => e.EditTime)
                    .HasColumnType("datetime")
                    .HasColumnName("edit_time")
                    .HasComment("编辑时间");

                entity.Property(e => e.NoteContent)
                    .HasMaxLength(255)
                    .HasColumnName("note_content")
                    .HasComment("跟踪内容");

                entity.Property(e => e.NoteWay)
                    .HasColumnName("note_way")
                    .HasComment("跟踪方式");

                entity.HasOne(d => d.Clue)
                    .WithMany(p => p.TClueRemarks)
                    .HasForeignKey(d => d.ClueId)
                    .HasConstraintName("t_clue_remark_ibfk_3");

                entity.HasOne(d => d.CreateByNavigation)
                    .WithMany(p => p.TClueRemarkCreateByNavigations)
                    .HasForeignKey(d => d.CreateBy)
                    .HasConstraintName("t_clue_remark_ibfk_1");

                entity.HasOne(d => d.EditByNavigation)
                    .WithMany(p => p.TClueRemarkEditByNavigations)
                    .HasForeignKey(d => d.EditBy)
                    .HasConstraintName("t_clue_remark_ibfk_2");

                entity.HasOne(d => d.NoteWayNavigation)
                    .WithMany(p => p.TClueRemarks)
                    .HasForeignKey(d => d.NoteWay)
                    .HasConstraintName("t_clue_remark_ibfk_4");

                entity.HasQueryFilter(c => c.Deleted != 1);
            });

            modelBuilder.Entity<TCustomer>(entity =>
            {
                entity.ToTable("t_customer");

                entity.HasComment("客户表")
                    .HasCharSet("utf8mb3")
                    .UseCollation("utf8mb3_general_ci");

                entity.HasIndex(e => e.ClueId, "t_customer_ibfk_1");

                entity.HasIndex(e => e.Product, "t_customer_ibfk_2");

                entity.HasIndex(e => e.CreateBy, "t_customer_ibfk_3");

                entity.HasIndex(e => e.EditBy, "t_customer_ibfk_4");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasComment("主键，自动增长，客户ID");

                entity.Property(e => e.ClueId)
                    .HasColumnName("clue_id")
                    .HasComment("线索ID");

                entity.Property(e => e.CreateBy)
                    .HasColumnName("create_by")
                    .HasComment("创建人");

                entity.Property(e => e.CreateTime)
                    .HasColumnType("datetime")
                    .HasColumnName("create_time")
                    .HasComment("创建时间");

                entity.Property(e => e.Description)
                    .HasMaxLength(255)
                    .HasColumnName("description")
                    .HasComment("客户描述");

                entity.Property(e => e.EditBy)
                    .HasColumnName("edit_by")
                    .HasComment("编辑人");

                entity.Property(e => e.EditTime)
                    .HasColumnType("datetime")
                    .HasColumnName("edit_time")
                    .HasComment("编辑时间");

                entity.Property(e => e.NextContactTime)
                    .HasColumnType("datetime")
                    .HasColumnName("next_contact_time")
                    .HasComment("下次联系时间");

                entity.Property(e => e.Product)
                    .HasColumnName("product")
                    .HasComment("选购产品");

                entity.HasOne(d => d.Clue)
                    .WithMany(p => p.TCustomers)
                    .HasForeignKey(d => d.ClueId)
                    .HasConstraintName("t_customer_ibfk_1");

                entity.HasOne(d => d.CreateByNavigation)
                    .WithMany(p => p.TCustomerCreateByNavigations)
                    .HasForeignKey(d => d.CreateBy)
                    .HasConstraintName("t_customer_ibfk_3");

                entity.HasOne(d => d.EditByNavigation)
                    .WithMany(p => p.TCustomerEditByNavigations)
                    .HasForeignKey(d => d.EditBy)
                    .HasConstraintName("t_customer_ibfk_4");

                entity.HasOne(d => d.ProductNavigation)
                    .WithMany(p => p.TCustomers)
                    .HasForeignKey(d => d.Product)
                    .HasConstraintName("t_customer_ibfk_2");
            });

            modelBuilder.Entity<TCustomerRemark>(entity =>
            {
                entity.ToTable("t_customer_remark");

                entity.HasComment("客户跟踪记录表")
                    .HasCharSet("utf8mb3")
                    .UseCollation("utf8mb3_general_ci");

                entity.HasIndex(e => e.CustomerId, "t_customer_remark_ibfk_1");

                entity.HasIndex(e => e.NoteWay, "t_customer_remark_ibfk_2");

                entity.HasIndex(e => e.CreateBy, "t_customer_remark_ibfk_3");

                entity.HasIndex(e => e.EditBy, "t_customer_remark_ibfk_4");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasComment("主键，自动增长，客户备注ID");

                entity.Property(e => e.CreateBy)
                    .HasColumnName("create_by")
                    .HasComment("跟踪人");

                entity.Property(e => e.CreateTime)
                    .HasColumnType("datetime")
                    .HasColumnName("create_time")
                    .HasComment("跟踪时间");

                entity.Property(e => e.CustomerId)
                    .HasColumnName("customer_id")
                    .HasComment("客户ID");

                entity.Property(e => e.Deleted)
                    .HasColumnName("deleted")
                    .HasComment("删除状态（0正常，1删除）");

                entity.Property(e => e.EditBy)
                    .HasColumnName("edit_by")
                    .HasComment("编辑人");

                entity.Property(e => e.EditTime)
                    .HasColumnType("datetime")
                    .HasColumnName("edit_time")
                    .HasComment("编辑时间");

                entity.Property(e => e.NoteContent)
                    .HasMaxLength(255)
                    .HasColumnName("note_content")
                    .HasComment("跟踪内容");

                entity.Property(e => e.NoteWay)
                    .HasColumnName("note_way")
                    .HasComment("跟踪方式");

                entity.HasOne(d => d.CreateByNavigation)
                    .WithMany(p => p.TCustomerRemarkCreateByNavigations)
                    .HasForeignKey(d => d.CreateBy)
                    .HasConstraintName("t_customer_remark_ibfk_3");

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.TCustomerRemarks)
                    .HasForeignKey(d => d.CustomerId)
                    .HasConstraintName("t_customer_remark_ibfk_1");

                entity.HasOne(d => d.EditByNavigation)
                    .WithMany(p => p.TCustomerRemarkEditByNavigations)
                    .HasForeignKey(d => d.EditBy)
                    .HasConstraintName("t_customer_remark_ibfk_4");

                entity.HasOne(d => d.NoteWayNavigation)
                    .WithMany(p => p.TCustomerRemarks)
                    .HasForeignKey(d => d.NoteWay)
                    .HasConstraintName("t_customer_remark_ibfk_2");
            });

            modelBuilder.Entity<TDicType>(entity =>
            {
                entity.ToTable("t_dic_type");

                entity.HasComment("字典类型表")
                    .HasCharSet("utf8mb3")
                    .UseCollation("utf8mb3_general_ci");

                entity.HasIndex(e => e.TypeCode, "code");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasComment("主键，自动增长，字典类型ID");

                entity.Property(e => e.Remark)
                    .HasMaxLength(128)
                    .HasColumnName("remark")
                    .HasComment("备注");

                entity.Property(e => e.TypeCode)
                    .HasMaxLength(64)
                    .HasColumnName("type_code")
                    .HasComment("字典类型代码");

                entity.Property(e => e.TypeName)
                    .HasMaxLength(64)
                    .HasColumnName("type_name")
                    .HasComment("字典类型名称");
            });

            modelBuilder.Entity<TDicValue>(entity =>
            {
                entity.ToTable("t_dic_value");

                entity.HasComment("字典值表")
                    .HasCharSet("utf8mb3")
                    .UseCollation("utf8mb3_general_ci");

                entity.HasIndex(e => e.TypeCode, "t_dic_value_ibfk_1");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasComment("主键，自动增长，字典值ID");

                entity.Property(e => e.Order)
                    .HasColumnName("order")
                    .HasComment("字典值排序");

                entity.Property(e => e.Remark)
                    .HasMaxLength(64)
                    .HasColumnName("remark")
                    .HasComment("备注");

                entity.Property(e => e.TypeCode)
                    .HasMaxLength(64)
                    .HasColumnName("type_code")
                    .HasComment("字典类型代码");

                entity.Property(e => e.TypeValue)
                    .HasMaxLength(64)
                    .HasColumnName("type_value")
                    .HasComment("字典值");
            });

            modelBuilder.Entity<TPermission>(entity =>
            {
                entity.ToTable("t_permission");

                entity.HasComment("权限表")
                    .UseCollation("utf8mb4_general_ci");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Code)
                    .HasMaxLength(30)
                    .HasColumnName("code");

                entity.Property(e => e.Icon)
                    .HasMaxLength(100)
                    .HasColumnName("icon");

                entity.Property(e => e.Name)
                    .HasMaxLength(30)
                    .HasColumnName("name");

                entity.Property(e => e.OrderNo).HasColumnName("order_no");

                entity.Property(e => e.ParentId).HasColumnName("parent_id");

                entity.Property(e => e.Type)
                    .HasMaxLength(30)
                    .HasColumnName("type");

                entity.Property(e => e.Url)
                    .HasMaxLength(30)
                    .HasColumnName("url");
            });

            modelBuilder.Entity<TProduct>(entity =>
            {
                entity.ToTable("t_product");

                entity.HasComment("产品表")
                    .UseCollation("utf8mb4_general_ci");

                entity.HasIndex(e => e.CreateBy, "t_product_ibfk_1");

                entity.HasIndex(e => e.EditBy, "t_product_ibfk_2");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasComment("主键，自动增长，线索ID");

                entity.Property(e => e.CreateBy)
                    .HasColumnName("create_by")
                    .HasComment("创建人");

                entity.Property(e => e.CreateTime)
                    .HasColumnType("datetime")
                    .HasColumnName("create_time")
                    .HasComment("创建时间");

                entity.Property(e => e.EditBy)
                    .HasColumnName("edit_by")
                    .HasComment("编辑人");

                entity.Property(e => e.EditTime)
                    .HasColumnType("datetime")
                    .HasColumnName("edit_time")
                    .HasComment("编辑时间");

                entity.Property(e => e.GuidePriceE)
                    .HasPrecision(10, 2)
                    .HasColumnName("guide_price_e")
                    .HasComment("官方指导最高价");

                entity.Property(e => e.GuidePriceS)
                    .HasPrecision(10, 2)
                    .HasColumnName("guide_price_s")
                    .HasComment("官方指导起始价");

                entity.Property(e => e.Name)
                    .HasMaxLength(128)
                    .HasColumnName("name")
                    .HasComment("产品名称");

                entity.Property(e => e.Quotation)
                    .HasPrecision(10, 2)
                    .HasColumnName("quotation")
                    .HasComment("经销商报价");

                entity.Property(e => e.State)
                    .HasColumnName("state")
                    .HasComment("状态 0在售 1售罄");

                entity.HasOne(d => d.CreateByNavigation)
                    .WithMany(p => p.TProductCreateByNavigations)
                    .HasForeignKey(d => d.CreateBy)
                    .HasConstraintName("t_product_ibfk_1");

                entity.HasOne(d => d.EditByNavigation)
                    .WithMany(p => p.TProductEditByNavigations)
                    .HasForeignKey(d => d.EditBy)
                    .HasConstraintName("t_product_ibfk_2");
            });

            modelBuilder.Entity<TRole>(entity =>
            {
                entity.ToTable("t_role");

                entity.HasComment("角色表")
                    .UseCollation("utf8mb4_general_ci");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Role)
                    .HasMaxLength(30)
                    .HasColumnName("role");

                entity.Property(e => e.RoleName)
                    .HasMaxLength(30)
                    .HasColumnName("role_name");
            });

            modelBuilder.Entity<TRolePermission>(entity =>
            {
                entity.ToTable("t_role_permission");

                entity.HasComment("角色权限关系表")
                    .UseCollation("utf8mb4_general_ci");

                entity.HasIndex(e => e.RoleId, "t_role_permission_ibfk_1");

                entity.HasIndex(e => e.PermissionId, "t_role_permission_ibfk_2");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.PermissionId).HasColumnName("permission_id");

                entity.Property(e => e.RoleId).HasColumnName("role_id");

                entity.HasOne(d => d.Permission)
                    .WithMany(p => p.TRolePermissions)
                    .HasForeignKey(d => d.PermissionId)
                    .HasConstraintName("t_role_permission_ibfk_2");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.TRolePermissions)
                    .HasForeignKey(d => d.RoleId)
                    .HasConstraintName("t_role_permission_ibfk_1");
            });

            modelBuilder.Entity<TSystemInfo>(entity =>
            {
                entity.ToTable("t_system_info");

                entity.HasComment("系统信息表")
                    .HasCharSet("utf8mb3")
                    .UseCollation("utf8mb3_general_ci");

                entity.HasIndex(e => e.CreateBy, "t_system_info_ibfk_1");

                entity.HasIndex(e => e.EditBy, "t_system_info_ibfk_2");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Address)
                    .HasMaxLength(100)
                    .HasColumnName("address");

                entity.Property(e => e.CloseMsg)
                    .HasMaxLength(500)
                    .HasColumnName("closeMsg");

                entity.Property(e => e.CreateBy).HasColumnName("create_by");

                entity.Property(e => e.CreateTime)
                    .HasColumnType("datetime")
                    .HasColumnName("create_time");

                entity.Property(e => e.Description)
                    .HasMaxLength(45)
                    .HasColumnName("description");

                entity.Property(e => e.EditBy).HasColumnName("edit_by");

                entity.Property(e => e.EditTime)
                    .HasColumnType("datetime")
                    .HasColumnName("edit_time");

                entity.Property(e => e.Email)
                    .HasMaxLength(45)
                    .HasColumnName("email");

                entity.Property(e => e.Isopen)
                    .HasMaxLength(8)
                    .HasColumnName("isopen")
                    .HasDefaultValueSql("'y'");

                entity.Property(e => e.Keywords)
                    .HasMaxLength(100)
                    .HasColumnName("keywords");

                entity.Property(e => e.Logo)
                    .HasMaxLength(100)
                    .HasColumnName("logo");

                entity.Property(e => e.Name)
                    .HasMaxLength(100)
                    .HasColumnName("name");

                entity.Property(e => e.Shortcuticon)
                    .HasMaxLength(100)
                    .HasColumnName("shortcuticon");

                entity.Property(e => e.Site)
                    .HasMaxLength(100)
                    .HasColumnName("site");

                entity.Property(e => e.SystemCode)
                    .HasMaxLength(45)
                    .HasColumnName("system_code");

                entity.Property(e => e.Tel)
                    .HasMaxLength(100)
                    .HasColumnName("tel");

                entity.Property(e => e.Title)
                    .HasMaxLength(45)
                    .HasColumnName("title");

                entity.Property(e => e.Version)
                    .HasMaxLength(145)
                    .HasColumnName("version");

                entity.Property(e => e.Weixin)
                    .HasMaxLength(25)
                    .HasColumnName("weixin");

                entity.HasOne(d => d.CreateByNavigation)
                    .WithMany(p => p.TSystemInfoCreateByNavigations)
                    .HasForeignKey(d => d.CreateBy)
                    .HasConstraintName("t_system_info_ibfk_1");

                entity.HasOne(d => d.EditByNavigation)
                    .WithMany(p => p.TSystemInfoEditByNavigations)
                    .HasForeignKey(d => d.EditBy)
                    .HasConstraintName("t_system_info_ibfk_2");
            });

            modelBuilder.Entity<TTran>(entity =>
            {
                entity.ToTable("t_tran");

                entity.HasComment("交易表")
                    .HasCharSet("utf8mb3")
                    .UseCollation("utf8mb3_general_ci");

                entity.HasIndex(e => e.CustomerId, "t_tran_ibfk_1");

                entity.HasIndex(e => e.Stage, "t_tran_ibfk_2");

                entity.HasIndex(e => e.CreateBy, "t_tran_ibfk_3");

                entity.HasIndex(e => e.EditBy, "t_tran_ibfk_4");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasComment("主键，自动增长，交易ID");

                entity.Property(e => e.CreateBy)
                    .HasColumnName("create_by")
                    .HasComment("创建人");

                entity.Property(e => e.CreateTime)
                    .HasColumnType("datetime")
                    .HasColumnName("create_time")
                    .HasComment("创建时间");

                entity.Property(e => e.CustomerId)
                    .HasColumnName("customer_id")
                    .HasComment("客户ID");

                entity.Property(e => e.Description)
                    .HasMaxLength(255)
                    .HasColumnName("description")
                    .HasComment("交易描述");

                entity.Property(e => e.EditBy)
                    .HasColumnName("edit_by")
                    .HasComment("编辑人");

                entity.Property(e => e.EditTime)
                    .HasColumnType("datetime")
                    .HasColumnName("edit_time")
                    .HasComment("编辑时间");

                entity.Property(e => e.ExpectedDate)
                    .HasColumnType("datetime")
                    .HasColumnName("expected_date")
                    .HasComment("预计成交日期");

                entity.Property(e => e.Money)
                    .HasPrecision(10, 2)
                    .HasColumnName("money")
                    .HasComment("交易金额");

                entity.Property(e => e.NextContactTime)
                    .HasColumnType("datetime")
                    .HasColumnName("next_contact_time")
                    .HasComment("下次联系时间");

                entity.Property(e => e.Stage)
                    .HasColumnName("stage")
                    .HasComment("交易所处阶段");

                entity.Property(e => e.TranNo)
                    .HasMaxLength(255)
                    .HasColumnName("tran_no")
                    .HasComment("交易流水号");

                entity.HasOne(d => d.CreateByNavigation)
                    .WithMany(p => p.TTranCreateByNavigations)
                    .HasForeignKey(d => d.CreateBy)
                    .HasConstraintName("t_tran_ibfk_3");

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.TTrans)
                    .HasForeignKey(d => d.CustomerId)
                    .HasConstraintName("t_tran_ibfk_1");

                entity.HasOne(d => d.EditByNavigation)
                    .WithMany(p => p.TTranEditByNavigations)
                    .HasForeignKey(d => d.EditBy)
                    .HasConstraintName("t_tran_ibfk_4");

                entity.HasOne(d => d.StageNavigation)
                    .WithMany(p => p.TTrans)
                    .HasForeignKey(d => d.Stage)
                    .HasConstraintName("t_tran_ibfk_2");
            });

            modelBuilder.Entity<TTranHistory>(entity =>
            {
                entity.ToTable("t_tran_history");

                entity.HasComment("交易历史记录表")
                    .HasCharSet("utf8mb3")
                    .UseCollation("utf8mb3_general_ci");

                entity.HasIndex(e => e.TranId, "t_tran_history_ibfk_1");

                entity.HasIndex(e => e.Stage, "t_tran_history_ibfk_2");

                entity.HasIndex(e => e.CreateBy, "t_tran_history_ibfk_3");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasComment("主键，自动增长，交易记录ID");

                entity.Property(e => e.CreateBy)
                    .HasColumnName("create_by")
                    .HasComment("创建人");

                entity.Property(e => e.CreateTime)
                    .HasColumnType("datetime")
                    .HasColumnName("create_time")
                    .HasComment("创建时间");

                entity.Property(e => e.ExpectedDate)
                    .HasColumnType("datetime")
                    .HasColumnName("expected_date")
                    .HasComment("交易预计成交时间");

                entity.Property(e => e.Money)
                    .HasPrecision(10, 2)
                    .HasColumnName("money")
                    .HasComment("交易金额");

                entity.Property(e => e.Stage)
                    .HasColumnName("stage")
                    .HasComment("交易阶段");

                entity.Property(e => e.TranId)
                    .HasColumnName("tran_id")
                    .HasComment("交易ID");

                entity.HasOne(d => d.CreateByNavigation)
                    .WithMany(p => p.TTranHistories)
                    .HasForeignKey(d => d.CreateBy)
                    .HasConstraintName("t_tran_history_ibfk_3");

                entity.HasOne(d => d.StageNavigation)
                    .WithMany(p => p.TTranHistories)
                    .HasForeignKey(d => d.Stage)
                    .HasConstraintName("t_tran_history_ibfk_2");

                entity.HasOne(d => d.Tran)
                    .WithMany(p => p.TTranHistories)
                    .HasForeignKey(d => d.TranId)
                    .HasConstraintName("t_tran_history_ibfk_1");
            });

            modelBuilder.Entity<TTranRemark>(entity =>
            {
                entity.ToTable("t_tran_remark");

                entity.HasComment("交易跟踪记录表")
                    .HasCharSet("utf8mb3")
                    .UseCollation("utf8mb3_general_ci");

                entity.HasIndex(e => e.TranId, "t_tran_remark_ibfk_1");

                entity.HasIndex(e => e.NoteWay, "t_tran_remark_ibfk_2");

                entity.HasIndex(e => e.CreateBy, "t_tran_remark_ibfk_3");

                entity.HasIndex(e => e.EditBy, "t_tran_remark_ibfk_4");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasComment("主键，自动增长，交易备注ID");

                entity.Property(e => e.CreateBy)
                    .HasColumnName("create_by")
                    .HasComment("跟踪人");

                entity.Property(e => e.CreateTime)
                    .HasColumnType("datetime")
                    .HasColumnName("create_time")
                    .HasComment("跟踪时间");

                entity.Property(e => e.Deleted)
                    .HasColumnName("deleted")
                    .HasComment("删除状态（0正常，1删除）");

                entity.Property(e => e.EditBy)
                    .HasColumnName("edit_by")
                    .HasComment("编辑人");

                entity.Property(e => e.EditTime)
                    .HasColumnType("datetime")
                    .HasColumnName("edit_time")
                    .HasComment("编辑时间");

                entity.Property(e => e.NoteContent)
                    .HasMaxLength(255)
                    .HasColumnName("note_content")
                    .HasComment("跟踪内容");

                entity.Property(e => e.NoteWay)
                    .HasColumnName("note_way")
                    .HasComment("跟踪方式");

                entity.Property(e => e.TranId)
                    .HasColumnName("tran_id")
                    .HasComment("交易ID");

                entity.HasOne(d => d.CreateByNavigation)
                    .WithMany(p => p.TTranRemarkCreateByNavigations)
                    .HasForeignKey(d => d.CreateBy)
                    .HasConstraintName("t_tran_remark_ibfk_3");

                entity.HasOne(d => d.EditByNavigation)
                    .WithMany(p => p.TTranRemarkEditByNavigations)
                    .HasForeignKey(d => d.EditBy)
                    .HasConstraintName("t_tran_remark_ibfk_4");

                entity.HasOne(d => d.NoteWayNavigation)
                    .WithMany(p => p.TTranRemarks)
                    .HasForeignKey(d => d.NoteWay)
                    .HasConstraintName("t_tran_remark_ibfk_2");

                entity.HasOne(d => d.Tran)
                    .WithMany(p => p.TTranRemarks)
                    .HasForeignKey(d => d.TranId)
                    .HasConstraintName("t_tran_remark_ibfk_1");
            });

            modelBuilder.Entity<TUser>(entity =>
            {
                entity.ToTable("t_user");

                entity.HasComment("用户表")
                    .HasCharSet("utf8mb3")
                    .UseCollation("utf8mb3_general_ci");

                entity.HasIndex(e => e.Email, "email")
                    .IsUnique();

                entity.HasIndex(e => e.LoginAct, "login_act")
                    .IsUnique();

                entity.HasIndex(e => e.Phone, "phone")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasComment("主键，自动增长，用户ID");

                entity.Property(e => e.AccountEnabled)
                    .HasColumnName("account_enabled")
                    .HasComment("账号是否启用，0禁用 1启用");

                entity.Property(e => e.AccountNoExpired)
                    .HasColumnName("account_no_expired")
                    .HasComment("账户是否没有过期，0已过期 1正常");

                entity.Property(e => e.AccountNoLocked)
                    .HasColumnName("account_no_locked")
                    .HasComment("账号是否没有锁定，0已锁定 1正常");

                entity.Property(e => e.CreateBy)
                    .HasColumnName("create_by")
                    .HasComment("创建人");

                entity.Property(e => e.CreateTime)
                    .HasColumnType("datetime")
                    .HasColumnName("create_time")
                    .HasComment("创建时间");

                entity.Property(e => e.CredentialsNoExpired)
                    .HasColumnName("credentials_no_expired")
                    .HasComment("密码是否没有过期，0已过期 1正常");

                entity.Property(e => e.EditBy)
                    .HasColumnName("edit_by")
                    .HasComment("编辑人");

                entity.Property(e => e.EditTime)
                    .HasColumnType("datetime")
                    .HasColumnName("edit_time")
                    .HasComment("编辑时间");

                entity.Property(e => e.Email)
                    .HasMaxLength(64)
                    .HasColumnName("email")
                    .HasComment("用户邮箱");

                entity.Property(e => e.Jwtversion)
                    .HasColumnName("JWTVersion")
                    .HasComment("JWTVersion");

                entity.Property(e => e.LastLoginTime)
                    .HasColumnType("datetime")
                    .HasColumnName("last_login_time")
                    .HasComment("最近登录时间");

                entity.Property(e => e.LoginAct)
                    .HasMaxLength(32)
                    .HasColumnName("login_act")
                    .HasComment("登录账号");

                entity.Property(e => e.LoginPwd)
                    .HasMaxLength(64)
                    .HasColumnName("login_pwd")
                    .HasComment("登录密码");

                entity.Property(e => e.Name)
                    .HasMaxLength(32)
                    .HasColumnName("name")
                    .HasComment("用户姓名");

                entity.Property(e => e.Phone)
                    .HasMaxLength(18)
                    .HasColumnName("phone")
                    .HasComment("用户手机");
            });

            modelBuilder.Entity<TUserRole>(entity =>
            {
                entity.ToTable("t_user_role");

                entity.HasComment("用户角色关系表")
                    .UseCollation("utf8mb4_general_ci");

                entity.HasIndex(e => e.UserId, "t_user_role_ibfk_1");

                entity.HasIndex(e => e.RoleId, "t_user_role_ibfk_2");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.RoleId).HasColumnName("role_id");

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.TUserRoles)
                    .HasForeignKey(d => d.RoleId)
                    .HasConstraintName("t_user_role_ibfk_2");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.TUserRoles)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("t_user_role_ibfk_1");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
