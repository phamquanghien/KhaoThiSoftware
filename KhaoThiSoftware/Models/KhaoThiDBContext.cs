namespace KhaoThiSoftware.Models
{
    using System.Data.Entity;

    public partial class KhaoThiDBContext : DbContext
    {
        public KhaoThiDBContext()
            : base("name=KhaoThiDBContext")
        {
        }

        public virtual DbSet<Account> Accounts { get; set; }
        public virtual DbSet<DanhSachPhach> DanhSachPhachs { get; set; }
        public virtual DbSet<DanhSachThi> DanhSachThis { get; set; }
        public virtual DbSet<KyThi> KyThis { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<KetQuaThi> KetQuaThis { get; set; }
        public virtual DbSet<TestNhapDiem> TestNhapDiems { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Account>()
                .Property(e => e.UserName)
                .IsUnicode(false);

            modelBuilder.Entity<Account>()
                .Property(e => e.Password)
                .IsUnicode(false);

            modelBuilder.Entity<Account>()
                .Property(e => e.RoleID)
                .IsUnicode(false);

            //modelBuilder.Entity<DanhSachPhach>()
            //    .Property(e => e.IdDanhSachPhach)
            //    .Identity(false);

            modelBuilder.Entity<KyThi>()
                .Property(e => e.MaKyThi)
                .IsUnicode(false);

            modelBuilder.Entity<Role>()
                .Property(e => e.RoleID)
                .IsUnicode(false);
        }
    }
}
