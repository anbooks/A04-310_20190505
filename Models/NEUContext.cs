using System;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace WebApplication8.Models
{
    public partial class NEUContext : DbContext
    {
        public virtual DbSet<DbBm> DbBm { get; set; }
        public virtual DbSet<DbBu> DbBu { get; set; }
        public virtual DbSet<DbEm> DbEm { get; set; }
        public virtual DbSet<DbExam> DbExam { get; set; }
        public virtual DbSet<DbLog> DbLog { get; set; }
        public virtual DbSet<DbPa> DbPa { get; set; }
        public virtual DbSet<DbPax> DbPax { get; set; }
        public virtual DbSet<DbPo> DbPo { get; set; }
        public virtual DbSet<DbQu> DbQu { get; set; }
        public virtual DbSet<DbSc> DbSc { get; set; }
        public virtual DbSet<DbSta> DbSta { get; set; }
        public virtual DbSet<DbSu1> DbSu1 { get; set; }
        public virtual DbSet<DbSu2> DbSu2 { get; set; }
        public virtual DbSet<DbSu3> DbSu3 { get; set; }
        public virtual DbSet<DbSu4> DbSu4 { get; set; }
        public virtual DbSet<DbSu5> DbSu5 { get; set; }
        public virtual DbSet<DbTe> DbTe { get; set; }
        public virtual DbSet<DbTick> DbTick { get; set; }
     
      
        public virtual DbSet<TeSc> TeSc { get; set; }
        public virtual DbSet<Test> Test { get; set; }
      
        public virtual DbSet<TestAn> TestAn { get; set; }
        public virtual DbSet<TestQu> TestQu { get; set; }
        public virtual DbSet<TeTe> TeTe { get; set; }
        public virtual DbSet<TeEm> TeEm { get; set; }
        public NEUContext(DbContextOptions<NEUContext> options)
            : base(options)
        {

        }
        public NEUContext()
        {

        }


        // Unable to generate entity type for table 'dbo.SACC_NEU_department_record'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.SACC_NEU_department_record_copy'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.kong'. Please see the warning messages.

       

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer(@"Server=192.168.80.143;Database=NEU;User id=sa;Password=123;Trusted_Connection=false;");
            }
        }

        /// <summary>
        /// 更新后的NEUContext
        /// </summary>
        /// <param name="构造模型类"></param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TeEm>(entity =>
            {
                entity.Property(e => e.TeFlag).HasColumnName("Te_Flag");

                entity.HasOne(d => d.Em)
                    .WithMany(p => p.TeEm)
                    .HasForeignKey(d => d.TeEmId)
                    .HasConstraintName("FK__TeEm__TeEmId__29221CFB");

                entity.HasOne(d => d.Te)
                    .WithMany(p => p.TeEm)
                    .HasForeignKey(d => d.Teid)
                    .HasConstraintName("FK__TeEm__Teid__2A164134");
            });
            modelBuilder.Entity<DbBm>(entity =>
            {
                entity.HasKey(e => e.Id);//20190430


                entity.ToTable("db_bm");

                // entity.Property(e => e.Id).ValueGeneratedNever();
                entity.Property(e => e.Id).HasColumnName("Id");

                entity.Property(e => e.Branch)
                    .HasColumnName("branch")
                    .HasMaxLength(50)
                    .IsUnicode(false);



                //entity.HasKey(e => e.PoId);

                //entity.ToTable("db_po");

                //entity.Property(e => e.PoId).HasColumnName("Po_ID");

                //entity.Property(e => e.PoName)
                //    .IsRequired()
                //    .HasColumnName("Po_Name")
                //    .HasMaxLength(20);



            });

            modelBuilder.Entity<DbBu>(entity =>
            {
                entity.HasKey(e => e.BuId);

                entity.ToTable("db_bu");

                entity.Property(e => e.BuId).HasColumnName("Bu_ID");

                entity.Property(e => e.BuName)
                    .IsRequired()
                    .HasColumnName("Bu_Name")
                    .HasMaxLength(20);
            });

            modelBuilder.Entity<DbEm>(entity =>
            {
                entity.HasKey(e => e.EmId);

                entity.ToTable("db_em");

                entity.Property(e => e.EmId).HasColumnName("Em_ID");

                entity.Property(e => e.Branch).HasDefaultValueSql("((0))");

                entity.Property(e => e.BuId)
                    .HasColumnName("Bu_ID")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.CardId)
                    .HasColumnName("CardID")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.EmName)
                    .HasColumnName("Em_Name")
                    .HasMaxLength(20);

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Password1)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Picture)
                    .HasColumnName("Picture")
                    .HasMaxLength(100);

                entity.Property(e => e.PoId)
                    .HasColumnName("Po_ID")
                    .HasDefaultValueSql("((0))");

                entity.HasOne(d => d.Bm)
                    .WithMany(p => p.DbEm)
                    .HasForeignKey(d => d.Branch)
                    .HasConstraintName("FK__db_em__Branch__74AE54BC");


                //entity.HasOne(d => d.Bm)
                //.WithMany(p => p.DbEm)
                //.HasForeignKey(d => d.Branch)
                //.HasConstraintName("FK__db_em__Branch__4183B671");

                entity.HasOne(d => d.Bu)
                    .WithMany(p => p.DbEm)
                    .HasForeignKey(d => d.BuId)
                    .HasConstraintName("FK__db_em__Bu_ID__75A278F5");

                entity.HasOne(d => d.Po)
                    .WithMany(p => p.DbEm)
                    .HasForeignKey(d => d.PoId)
                    .HasConstraintName("FK__db_em__Po_ID__76969D2E");
            });

            modelBuilder.Entity<DbExam>(entity =>
            {
                entity.HasKey(e => e.ExamId);

                entity.ToTable("db_exam");

                entity.Property(e => e.ExamId).HasColumnName("ExamID");

                entity.Property(e => e.EmAnswer)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.OptionA).HasMaxLength(50);

                entity.Property(e => e.OptionB).HasMaxLength(50);

                entity.Property(e => e.OptionC).HasMaxLength(50);

                entity.Property(e => e.OptionD).HasMaxLength(50);

                entity.Property(e => e.PaId).HasColumnName("Pa_ID");

                entity.Property(e => e.Pax1_ID).HasColumnName("Pax1_ID");

                entity.Property(e => e.QuId).HasColumnName("Qu_ID");

                entity.Property(e => e.Question).HasMaxLength(50);

                entity.Property(e => e.RightAnswer).HasMaxLength(50);

                entity.Property(e => e.RW)
                    .HasColumnName("RW")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Type).HasMaxLength(50);

                entity.HasOne(d => d.Pa)
                    .WithMany(p => p.DbExam)
                    .HasForeignKey(d => d.PaId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__db_exam__Pa_ID__67DE6983");
            });

            modelBuilder.Entity<DbLog>(entity =>
            {
                entity.HasKey(e => e.AlterId);

                entity.ToTable("db_log");

                entity.Property(e => e.AlterId).HasColumnName("Alter_ID");

                entity.Property(e => e.AlDifficulty)
                    .IsRequired()
                    .HasColumnName("Al_Difficulty")
                    .HasMaxLength(5)
                    .IsUnicode(false);

                entity.Property(e => e.AlEdit)
                    .HasColumnName("Al_Edit")
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.AlOptionA)
                    .IsRequired()
                    .HasColumnName("Al_OptionA")
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.AlOptionB)
                    .IsRequired()
                    .HasColumnName("Al_OptionB")
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.AlOptionC)
                    .IsRequired()
                    .HasColumnName("Al_OptionC")
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.AlOptionD)
                    .IsRequired()
                    .HasColumnName("Al_OptionD")
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.AlQuestion)
                    .IsRequired()
                    .HasColumnName("Al_Question");

                entity.Property(e => e.AlRightAnswer)
                    .IsRequired()
                    .HasColumnName("Al_RightAnswer")
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.AlterTime).HasColumnType("datetime");

                entity.Property(e => e.EmName)
                    .IsRequired()
                    .HasColumnName("Em_Name")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.QuId).HasColumnName("Qu_ID");
            });

            modelBuilder.Entity<DbPa>(entity =>
            {
                entity.HasKey(e => e.PaId);

                entity.ToTable("db_pa");

                entity.Property(e => e.PaId).HasColumnName("Pa_ID");

                entity.Property(e => e.EmId).HasColumnName("Em_ID");

                entity.Property(e => e.Tester).HasColumnType("nchar(10)");

                entity.HasOne(d => d.Em)
                    .WithMany(p => p.DbPa)
                    .HasForeignKey(d => d.EmId)
                    .HasConstraintName("FK__db_pa__Em_ID__6E8B6712");

                entity.HasOne(d => d.Sua)
                    .WithMany(p => p.DbPa)
                    .HasForeignKey(d => d.SuaId)
                    .HasConstraintName("FK__db_pa__SuaId__6BAEFA67");

                entity.HasOne(d => d.Sub)
                    .WithMany(p => p.DbPa)
                    .HasForeignKey(d => d.SubId)
                    .HasConstraintName("FK__db_pa__SubId__6CA31EA0");

                entity.HasOne(d => d.Suc)
                    .WithMany(p => p.DbPa)
                    .HasForeignKey(d => d.SucId)
                    .HasConstraintName("FK__db_pa__SucId__6D9742D9");
            });

            modelBuilder.Entity<DbPax>(entity =>
            {
                entity.HasKey(e => e.PaxId);

                entity.ToTable("db_pax");

                entity.Property(e => e.PaxId).HasColumnName("Pax_ID");

                entity.Property(e => e.PaId).HasColumnName("Pa_ID");

                entity.Property(e => e.Pax1_ID).HasColumnName("Pax1_ID");

                entity.Property(e => e.QuId).HasColumnName("Qu_ID");

                entity.HasOne(d => d.Pa)
                    .WithMany(p => p.DbPax)
                    .HasForeignKey(d => d.PaId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__db_pax__Pa_ID__6ABAD62E");

                entity.HasOne(d => d.Qu)
                    .WithMany(p => p.DbPax)
                    .HasForeignKey(d => d.QuId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__db_pax__Qu_ID__019E3B86");
            });

            modelBuilder.Entity<DbPo>(entity =>
            {
                entity.HasKey(e => e.PoId);

                entity.ToTable("db_po");

                entity.Property(e => e.PoId).HasColumnName("Po_ID");

                entity.Property(e => e.PoName)
                    .IsRequired()
                    .HasColumnName("Po_Name")
                    .HasMaxLength(20);
            });

            modelBuilder.Entity<DbQu>(entity =>
            {
                entity.HasKey(e => e.QuId);

                entity.ToTable("db_qu");

                entity.Property(e => e.QuId).HasColumnName("Qu_ID");

                entity.Property(e => e.BuId).HasColumnName("Bu_ID");

                entity.Property(e => e.Difficulty)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.EmId).HasColumnName("Em_ID");

                entity.Property(e => e.OptionA)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.OptionB)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.OptionC)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.OptionD)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.Question)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.RightAnswer)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.SuaId).HasColumnName("SuaID");

                entity.Property(e => e.SubId).HasColumnName("SubID");

                entity.Property(e => e.SucId).HasColumnName("SucID");

                entity.Property(e => e.SudId).HasColumnName("SudID");

                entity.Property(e => e.SueId).HasColumnName("SueID");

                entity.Property(e => e.Type)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.HasOne(d => d.Bu)
                    .WithMany(p => p.DbQu)
                    .HasForeignKey(d => d.BuId)
                    .HasConstraintName("FK__db_qu__Bu_ID__408F9238");

                entity.HasOne(d => d.Em)
                    .WithMany(p => p.DbQu)
                    .HasForeignKey(d => d.EmId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__db_qu__Em_ID__7AF13DF7");

                entity.HasOne(d => d.Sua)
                    .WithMany(p => p.DbQu)
                    .HasForeignKey(d => d.SuaId)
                    .HasConstraintName("FK__db_qu__SuaID__7DCDAAA2");

                entity.HasOne(d => d.Sub)
                    .WithMany(p => p.DbQu)
                    .HasForeignKey(d => d.SubId)
                    .HasConstraintName("FK__db_qu__SubID__7EC1CEDB");

                entity.HasOne(d => d.Suc)
                    .WithMany(p => p.DbQu)
                    .HasForeignKey(d => d.SucId)
                    .HasConstraintName("FK__db_qu__SucID__7BE56230");

                entity.HasOne(d => d.Sud)
                    .WithMany(p => p.DbQu)
                    .HasForeignKey(d => d.SudId)
                    .HasConstraintName("FK__db_qu__SudID__7FB5F314");

                entity.HasOne(d => d.Sue)
                    .WithMany(p => p.DbQu)
                    .HasForeignKey(d => d.SueId)
                    .HasConstraintName("FK__db_qu__SueID__00AA174D");
            });

            modelBuilder.Entity<DbSc>(entity =>
            {
                entity.HasKey(e => e.ScId);

                entity.ToTable("db_sc");

                entity.Property(e => e.ScId).HasColumnName("Sc_ID");

                entity.Property(e => e.BuId).HasColumnName("Bu_ID");

                entity.Property(e => e.EmId).HasColumnName("Em_ID");

                entity.Property(e => e.PaId).HasColumnName("Pa_ID");

                entity.Property(e => e.TestEnd).HasColumnType("datetime");

                entity.Property(e => e.TestStart).HasColumnType("datetime");

                entity.HasOne(d => d.Bu)
                    .WithMany(p => p.DbSc)
                    .HasForeignKey(d => d.BuId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__db_sc__Bu_ID__3F9B6DFF");

                entity.HasOne(d => d.Em)
                    .WithMany(p => p.DbSc)
                    .HasForeignKey(d => d.EmId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__db_sc__Em_ID__25518C17");

                entity.HasOne(d => d.Pa)
                    .WithMany(p => p.DbSc)
                    .HasForeignKey(d => d.PaId)
                    .HasConstraintName("FK__db_sc__Pa_ID__68D28DBC");
            });

            modelBuilder.Entity<DbSta>(entity =>
            {
                entity.HasKey(e => e.StaId);

                entity.ToTable("db_sta");

                entity.Property(e => e.StaId).HasColumnName("Sta_Id");

                entity.Property(e => e.EmId).HasColumnName("Em_ID");

                entity.Property(e => e.QueryNum)
                    .HasColumnName("Query_num")
                    .HasMaxLength(50);

                entity.HasOne(d => d.Em)
                    .WithMany(p => p.DbSta)
                    .HasForeignKey(d => d.EmId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__db_sta__Em_ID__14E61A24");
            });

            modelBuilder.Entity<DbSu1>(entity =>
            {
                entity.HasKey(e => e.SuaId);

                entity.ToTable("db_su1");

                entity.Property(e => e.SuaId).HasColumnName("SuaID");

                entity.Property(e => e.SuaName)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<DbSu2>(entity =>
            {
                entity.HasKey(e => e.SubId);

                entity.ToTable("db_su2");

                entity.Property(e => e.SubId).HasColumnName("SubID");

                entity.Property(e => e.SuaId).HasColumnName("SuaID");

                entity.Property(e => e.SubName)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.HasOne(d => d.Sua)
                    .WithMany(p => p.DbSu2)
                    .HasForeignKey(d => d.SuaId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__db_su2__SuaID__2AD55B43");
            });

            modelBuilder.Entity<DbSu3>(entity =>
            {
                entity.HasKey(e => e.SucId);

                entity.ToTable("db_su3");

                entity.Property(e => e.SucId).HasColumnName("SucID");

                entity.Property(e => e.SubId).HasColumnName("SubID");

                entity.Property(e => e.SucName)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.HasOne(d => d.Sub)
                    .WithMany(p => p.DbSu3)
                    .HasForeignKey(d => d.SubId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__db_su3__SubID__29E1370A");
            });

            modelBuilder.Entity<DbSu4>(entity =>
            {
                entity.HasKey(e => e.SudId);

                entity.ToTable("db_su4");

                entity.Property(e => e.SudId).HasColumnName("SudID");

                entity.Property(e => e.SucId).HasColumnName("SucID");

                entity.Property(e => e.SudName)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.HasOne(d => d.Suc)
                    .WithMany(p => p.DbSu4)
                    .HasForeignKey(d => d.SucId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__db_su4__SucID__3DE82FB7");
            });

            modelBuilder.Entity<DbSu5>(entity =>
            {
                entity.HasKey(e => e.SueId);

                entity.ToTable("db_su5");

                entity.Property(e => e.SueId).HasColumnName("SueID");

                entity.Property(e => e.SudId).HasColumnName("SudID");

                entity.Property(e => e.SueName)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.HasOne(d => d.Sud)
                    .WithMany(p => p.DbSu5)
                    .HasForeignKey(d => d.SudId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__db_su5__SudID__3EDC53F0");
            });

            modelBuilder.Entity<DbTe>(entity =>
            {
                entity.HasKey(e => e.TeId);

                entity.ToTable("db_te");

                entity.Property(e => e.TeId).HasColumnName("Te_ID");

                entity.Property(e => e.EmAnswer)
                    .HasColumnName("Em_Answer")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.PaId).HasColumnName("Pa_ID");

                entity.Property(e => e.PaxId).HasColumnName("Pax_ID");

                entity.Property(e => e.RW)
                    .HasColumnName("RW")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.Pa)
                    .WithMany(p => p.DbTe)
                    .HasForeignKey(d => d.PaId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__db_te__Pa_ID__69C6B1F5");

                entity.HasOne(d => d.Pax)
                    .WithMany(p => p.DbTe)
                    .HasForeignKey(d => d.PaxId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__db_te__Pax_ID__00DF2177");
            });

            modelBuilder.Entity<DbTick>(entity =>
            {
                entity.HasKey(e => e.TickId);

                entity.ToTable("db_tick");

                entity.Property(e => e.EmName).HasMaxLength(50);

                entity.Property(e => e.Handle).HasMaxLength(50);

                entity.Property(e => e.Ticking).HasMaxLength(50);

                entity.Property(e => e.Update).HasMaxLength(50);


                entity.HasOne(d => d.Qu)
                    .WithMany(p => p.DbTick)
                    .HasForeignKey(d => d.QuId)
                    .HasConstraintName("FK__db_tick__QuId__74AE54BC");
            });

          
          

            modelBuilder.Entity<TeSc>(entity =>
            {
                entity.HasKey(e => e.ScId);

                entity.ToTable("te_sc");

                entity.Property(e => e.ScId).HasColumnName("Sc_ID");

                entity.Property(e => e.BuId).HasColumnName("Bu_ID");

                entity.Property(e => e.EmId).HasColumnName("Em_ID");

                entity.Property(e => e.PaId).HasColumnName("Pa_ID");

                entity.Property(e => e.TestEnd).HasColumnType("datetime");

                entity.Property(e => e.TestStart).HasColumnType("datetime");

                entity.HasOne(d => d.Bu)
                    .WithMany(p => p.TeSc)
                    .HasForeignKey(d => d.BuId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__te_sc__Bu_ID__3EA749C6");

                entity.HasOne(d => d.Em)
                    .WithMany(p => p.TeSc)
                    .HasForeignKey(d => d.EmId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__te_sc__Em_ID__324172E1");

                entity.HasOne(d => d.Pa)
                    .WithMany(p => p.TeSc)
                    .HasForeignKey(d => d.PaId)
                    .HasConstraintName("FK__te_sc__Pa_ID__3429BB53");
            });

            modelBuilder.Entity<Test>(entity =>
            {
                entity.Property(e => e.MobanFlag).HasColumnName("Moban_flag");

                entity.Property(e => e.ShenpiFlag).HasColumnName("Shenpi_flag");

                entity.Property(e => e.TestName).IsRequired();

                entity.Property(e => e.Tester).IsRequired();
                entity.Property(e => e.Adress).HasMaxLength(50);

                entity.Property(e => e.EndTime).HasColumnType("datetime");
       
                entity.Property(e => e.StartTime).HasColumnType("datetime");
            });


           

            modelBuilder.Entity<TestAn>(entity =>
            {
                entity.HasKey(e => e.TeAnId);

                entity.Property(e => e.EmId).HasColumnName("EmID");

                entity.Property(e => e.QuId).HasColumnName("QuID");

                entity.Property(e => e.RW)
                    .HasColumnName("RW")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.Em)
                    .WithMany(p => p.TestAn)
                    .HasForeignKey(d => d.EmId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__TestAn__EmID__27C3E46E");

                entity.HasOne(d => d.Qu)
                    .WithMany(p => p.TestAn)
                    .HasForeignKey(d => d.QuId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__TestAn__QuID__28B808A7");

                entity.HasOne(d => d.Tequ)
                    .WithMany(p => p.TestAn)
                    .HasForeignKey(d => d.TequId)
                    .HasConstraintName("FK__TestAn__TequId__26CFC035");

                entity.HasOne(d => d.Test)
                    .WithMany(p => p.TestAn)
                    .HasForeignKey(d => d.TestId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__TestAn__TestId__29AC2CE0");
            });

            modelBuilder.Entity<TestQu>(entity =>
            {
                entity.HasKey(e => e.TequId);

                entity.Property(e => e.QuId).HasColumnName("QuID");

                entity.Property(e => e.TestId).HasColumnName("TestID");

                entity.HasOne(d => d.Qu)
                    .WithMany(p => p.TestQu)
                    .HasForeignKey(d => d.QuId)
                    .HasConstraintName("FK__TestQu__QuID__24E777C3");

                entity.HasOne(d => d.Test)
                    .WithMany(p => p.TestQu)
                    .HasForeignKey(d => d.TestId)
                    .HasConstraintName("FK__TestQu__TestID__25DB9BFC");
            });

            modelBuilder.Entity<TeTe>(entity =>
            {
                entity.HasKey(e => e.TeId);

                entity.ToTable("te_te");

                entity.Property(e => e.TeId).HasColumnName("Te_ID");

                entity.Property(e => e.EmAnswer)
                    .HasColumnName("Em_Answer")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.PaId).HasColumnName("Pa_ID");

                entity.Property(e => e.PaxId).HasColumnName("Pax_ID");

                entity.Property(e => e.RW)
                    .HasColumnName("RW")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.Pa)
                    .WithMany(p => p.TeTe)
                    .HasForeignKey(d => d.PaId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__te_te__Pa_ID__351DDF8C");
            });
        }
    }
}
