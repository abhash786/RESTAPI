using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace RManjusha.RestServices.Models
{
    public partial class RManjushaContext : DbContext
    {
        public RManjushaContext()
        {
        }

        public RManjushaContext(DbContextOptions<RManjushaContext> options)
            : base(options)
        {
        }

        public virtual DbSet<BusinessStream> BusinessStreams { get; set; }
        public virtual DbSet<CourseMaster> CourseMasters { get; set; }
        public virtual DbSet<EducationDetail> EducationDetails { get; set; }
        public virtual DbSet<EmployerProfile> EmployerProfiles { get; set; }
        public virtual DbSet<EmployerTypeMaster> EmployerTypeMasters { get; set; }
        public virtual DbSet<ExperienceDetail> ExperienceDetails { get; set; }
        public virtual DbSet<IncorporationType> IncorporationTypes { get; set; }
        public virtual DbSet<JobPost> JobPosts { get; set; }
        public virtual DbSet<JobPostActivity> JobPostActivities { get; set; }
        public virtual DbSet<JobType> JobTypes { get; set; }
        public virtual DbSet<LocationMaster> LocationMasters { get; set; }
        public virtual DbSet<SeekerProfile> SeekerProfiles { get; set; }
        public virtual DbSet<SeekerType> SeekerTypes { get; set; }
        public virtual DbSet<SeekersSkillsSet> SeekersSkillsSets { get; set; }
        public virtual DbSet<SkillsSet> SkillsSets { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=tcp:rmanjusha.database.windows.net,1433;Initial Catalog=R-manjusha;Persist Security Info=False;User ID=dbadmin;Password=Rmanjusha@2021;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;;MultipleActiveResultSets=True");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<BusinessStream>(entity =>
            {
                entity.ToTable("business_Stream");

                entity.Property(e => e.BusinessStreamId).HasColumnName("businessStreamId");

                entity.Property(e => e.BusinessStreamName)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("businessStreamName");
            });

            modelBuilder.Entity<CourseMaster>(entity =>
            {
                entity.HasKey(e => e.CourseId)
                    .HasName("PK_course_Master_courseId");

                entity.ToTable("course_Master");

                entity.Property(e => e.CourseId).HasColumnName("courseId");

                entity.Property(e => e.CourseFullName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("courseFullName");

                entity.Property(e => e.CourseShortName)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("courseShortName");
            });

            modelBuilder.Entity<EducationDetail>(entity =>
            {
                entity.ToTable("education_Details");

                entity.HasIndex(e => new { e.SkrId, e.SkrCode, e.CourseId }, "UK_education_Details_IdCodeCourse")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CourseCompletionDate)
                    .HasColumnType("date")
                    .HasColumnName("courseCompletionDate");

                entity.Property(e => e.CourseId).HasColumnName("courseId");

                entity.Property(e => e.CourseSpecialization)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("courseSpecialization");

                entity.Property(e => e.CourseStartDate)
                    .HasColumnType("date")
                    .HasColumnName("courseStartDate");

                entity.Property(e => e.InstituteName)
                    .IsRequired()
                    .HasMaxLength(250)
                    .IsUnicode(false)
                    .HasColumnName("instituteName");

                entity.Property(e => e.OtherCourseName)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("otherCourseName");

                entity.Property(e => e.PercentageOrCgpa).HasColumnName("percentageOrCgpa");

                entity.Property(e => e.SkrCode)
                    .HasMaxLength(12)
                    .IsUnicode(false)
                    .HasColumnName("skrCode");

                entity.Property(e => e.SkrId).HasColumnName("skrId");

                entity.Property(e => e.UniversityBoardName)
                    .IsRequired()
                    .HasMaxLength(250)
                    .IsUnicode(false)
                    .HasColumnName("universityBoardName");

                entity.HasOne(d => d.Course)
                    .WithMany(p => p.EducationDetails)
                    .HasForeignKey(d => d.CourseId)
                    .HasConstraintName("FK_education_Details_courseId");

                entity.HasOne(d => d.SkrCodeNavigation)
                    .WithMany(p => p.EducationDetailSkrCodeNavigations)
                    .HasPrincipalKey(p => p.SkrCode)
                    .HasForeignKey(d => d.SkrCode)
                    .HasConstraintName("FK_education_Details_skrCode");

                entity.HasOne(d => d.Skr)
                    .WithMany(p => p.EducationDetailSkrs)
                    .HasForeignKey(d => d.SkrId)
                    .HasConstraintName("FK_education_Details_skrId");
            });

            modelBuilder.Entity<EmployerProfile>(entity =>
            {
                entity.HasKey(e => e.EmpId)
                    .HasName("PK_Employer_Profile_empId");

                entity.ToTable("employer_Profile");

                entity.HasIndex(e => e.EmpCode, "UK_Employer_Profile_EmpCode")
                    .IsUnique();

                entity.Property(e => e.EmpId)
                    .HasColumnName("empId")
                    .HasDefaultValueSql("(format(getdate(),'yyMMdd')+right('000'+CONVERT([nvarchar](4),NEXT VALUE FOR [dbo].[sequence_Employer]),(4)))");

                entity.Property(e => e.BusinessStreamId).HasColumnName("businessStreamId");

                entity.Property(e => e.CompanyLogoImage)
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .HasColumnName("companyLogoImage");

                entity.Property(e => e.EmpAboutUs)
                    .HasMaxLength(4000)
                    .HasColumnName("empAboutUs");

                entity.Property(e => e.EmpAddress)
                    .IsRequired()
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .HasColumnName("empAddress");

                entity.Property(e => e.EmpCode)
                    .IsRequired()
                    .HasMaxLength(12)
                    .IsUnicode(false)
                    .HasColumnName("empCode")
                    .HasDefaultValueSql("(('S-'+format(getdate(),'yyMMdd'))+right('000'+CONVERT([nvarchar](4),NEXT VALUE FOR [dbo].[sequence_Seeker]),(4)))");

                entity.Property(e => e.EmpContactNo)
                    .HasColumnType("numeric(15, 0)")
                    .HasColumnName("empContactNo");

                entity.Property(e => e.EmpContactPersonAltNumber)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("empContactPersonAltNumber");

                entity.Property(e => e.EmpContactPersonName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("empContactPersonName");

                entity.Property(e => e.EmpContactPersonNumber)
                    .HasColumnType("numeric(15, 0)")
                    .HasColumnName("empContactPersonNumber");

                entity.Property(e => e.EmpDepartment)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("empDepartment");

                entity.Property(e => e.EmpEmailId)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("empEmailId");

                entity.Property(e => e.EmpFullName)
                    .IsRequired()
                    .HasMaxLength(300)
                    .IsUnicode(false)
                    .HasColumnName("empFullName");

                entity.Property(e => e.EmpGstin)
                    .IsRequired()
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("empGSTIN");

                entity.Property(e => e.EmpIncorporationDate)
                    .HasColumnType("datetime")
                    .HasColumnName("empIncorporationDate");

                entity.Property(e => e.EmpPan)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("empPAN");

                entity.Property(e => e.EmpProfileCreationDate)
                    .HasColumnType("datetime")
                    .HasColumnName("empProfileCreationDate")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.EmpTypeId).HasColumnName("empTypeId");

                entity.Property(e => e.EmpWebsite)
                    .HasMaxLength(200)
                    .HasColumnName("empWebsite");

                entity.Property(e => e.FbId)
                    .HasMaxLength(200)
                    .HasColumnName("fbId");

                entity.Property(e => e.IncId).HasColumnName("incId");

                entity.Property(e => e.InstaId)
                    .HasMaxLength(200)
                    .HasColumnName("instaId");

                entity.Property(e => e.LastModifiedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.LkdnId)
                    .HasMaxLength(200)
                    .HasColumnName("lkdnId");

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(200)
                    .HasColumnName("password");

                entity.Property(e => e.TwtrId)
                    .HasMaxLength(200)
                    .HasColumnName("twtrId");

                entity.Property(e => e.WeGrowRegDate)
                    .HasColumnType("datetime")
                    .HasColumnName("weGrowRegDate")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.WeValueRegDate)
                    .HasColumnType("datetime")
                    .HasColumnName("weValueRegDate")
                    .HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.BusinessStream)
                    .WithMany(p => p.EmployerProfiles)
                    .HasForeignKey(d => d.BusinessStreamId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Employer_Profile_businessStreamId");

                entity.HasOne(d => d.EmpType)
                    .WithMany(p => p.EmployerProfiles)
                    .HasForeignKey(d => d.EmpTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Employer_Profile_empTypeId");

                entity.HasOne(d => d.Inc)
                    .WithMany(p => p.EmployerProfiles)
                    .HasForeignKey(d => d.IncId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Employer_Profile_incId");
            });

            modelBuilder.Entity<EmployerTypeMaster>(entity =>
            {
                entity.HasKey(e => e.EmpTypeId)
                    .HasName("PK_employer_Type_Master_empTypeId");

                entity.ToTable("employer_Type_Master");

                entity.Property(e => e.EmpTypeId)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("empTypeId");

                entity.Property(e => e.EmpTypeDesc)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("empTypeDesc");
            });

            modelBuilder.Entity<ExperienceDetail>(entity =>
            {
                entity.ToTable("experience_Details");

                entity.HasIndex(e => new { e.SkrId, e.SkrCode, e.SkrTypeId, e.JoiningDate }, "UK_experience_Details_IdCourseTypeJDate")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.EmpName)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("empName");

                entity.Property(e => e.ExpType)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("expType");

                entity.Property(e => e.IsCurrentJob).HasColumnName("isCurrentJob");

                entity.Property(e => e.JobCity)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("jobCity");

                entity.Property(e => e.JobCountry)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("jobCountry");

                entity.Property(e => e.JobProjectDesc)
                    .IsRequired()
                    .HasMaxLength(1000)
                    .HasColumnName("jobProjectDesc");

                entity.Property(e => e.JobState)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("jobState");

                entity.Property(e => e.JobTitle)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("jobTitle");

                entity.Property(e => e.JoiningDate)
                    .HasColumnType("date")
                    .HasColumnName("joiningDate");

                entity.Property(e => e.LeavingDate)
                    .HasColumnType("date")
                    .HasColumnName("leavingDate");

                entity.Property(e => e.SkrCode)
                    .IsRequired()
                    .HasMaxLength(12)
                    .IsUnicode(false)
                    .HasColumnName("skrCode");

                entity.Property(e => e.SkrId).HasColumnName("skrId");

                entity.Property(e => e.SkrTypeId).HasColumnName("skrTypeId");

                entity.HasOne(d => d.SkrCodeNavigation)
                    .WithMany(p => p.ExperienceDetailSkrCodeNavigations)
                    .HasPrincipalKey(p => p.SkrCode)
                    .HasForeignKey(d => d.SkrCode)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_experience_Details_skrCode");

                entity.HasOne(d => d.Skr)
                    .WithMany(p => p.ExperienceDetailSkrs)
                    .HasForeignKey(d => d.SkrId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_experience_Details_skrId");
            });

            modelBuilder.Entity<IncorporationType>(entity =>
            {
                entity.HasKey(e => e.IncId)
                    .HasName("PK_incorporation_Type_IncId");

                entity.ToTable("incorporation_Type");

                entity.Property(e => e.IncId)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("incId");

                entity.Property(e => e.IncDesc)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("incDesc");
            });

            modelBuilder.Entity<JobPost>(entity =>
            {
                entity.HasKey(e => e.JobPostingId)
                    .HasName("PK_job_Post_jobPostingID");

                entity.ToTable("job_Post");

                entity.HasIndex(e => e.JobPostingCode, "UK_job_Post_jobPostingCode")
                    .IsUnique();

                entity.Property(e => e.JobPostingId)
                    .HasColumnName("jobPostingID")
                    .HasDefaultValueSql("(format(getdate(),'yyMMdd')+right('000'+CONVERT([nvarchar](4),NEXT VALUE FOR [dbo].[sequence_jobPost]),(4)))");

                entity.Property(e => e.DesiredEdu)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("desiredEdu");

                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("id");

                entity.Property(e => e.IsCompanyNameHidden).HasColumnName("isCompanyNameHidden");

                entity.Property(e => e.IsJobActive).HasColumnName("isJobActive");

                entity.Property(e => e.JobCreatedDate)
                    .HasColumnType("date")
                    .HasColumnName("jobCreatedDate")
                    .HasDefaultValueSql("(CONVERT([date],getdate()))");

                entity.Property(e => e.JobDescription)
                    .IsRequired()
                    .HasMaxLength(8000)
                    .IsUnicode(false)
                    .HasColumnName("jobDescription");

                entity.Property(e => e.JobLocationId).HasColumnName("jobLocationID");

                entity.Property(e => e.JobPostTypeId).HasColumnName("jobPostTypeID");

                entity.Property(e => e.JobPostingCode)
                    .IsRequired()
                    .HasMaxLength(12)
                    .IsUnicode(false)
                    .HasColumnName("jobPostingCode")
                    .HasDefaultValueSql("(('S-'+format(getdate(),'yyMMdd'))+right('000'+CONVERT([nvarchar](4),NEXT VALUE FOR [dbo].[sequence_jobPost]),(4)))");

                entity.Property(e => e.JobPrimarySkill)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("jobPrimarySkill");

                entity.Property(e => e.JobSecondarySkill)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("jobSecondarySkill");

                entity.Property(e => e.JobTitle)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("jobTitle");

                entity.Property(e => e.MaxExp).HasColumnName("maxExp");

                entity.Property(e => e.MinExp).HasColumnName("minExp");

                entity.Property(e => e.PkgRangeFrom).HasColumnName("pkgRangeFrom");

                entity.Property(e => e.PkgRangeTo).HasColumnName("pkgRangeTo");

                entity.Property(e => e.PostedByEmpId).HasColumnName("postedByEmpId");

                entity.HasOne(d => d.JobLocation)
                    .WithMany(p => p.JobPosts)
                    .HasForeignKey(d => d.JobLocationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_job_Post_jobLocationID");

                entity.HasOne(d => d.JobPostType)
                    .WithMany(p => p.JobPosts)
                    .HasForeignKey(d => d.JobPostTypeId)
                    .HasConstraintName("FK_job_Post_jobPostTypeId");

                entity.HasOne(d => d.PostedByEmp)
                    .WithMany(p => p.JobPosts)
                    .HasForeignKey(d => d.PostedByEmpId)
                    .HasConstraintName("FK_job_Post_postedByEmpId");
            });

            modelBuilder.Entity<JobPostActivity>(entity =>
            {
                entity.ToTable("job_Post_Activity");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.ApplicantApplyDate)
                    .HasColumnType("datetime")
                    .HasColumnName("applicantApplyDate");

                entity.Property(e => e.JobPostingId).HasColumnName("jobPostingId");

                entity.Property(e => e.SkrId).HasColumnName("skrId");

                entity.HasOne(d => d.JobPosting)
                    .WithMany(p => p.JobPostActivities)
                    .HasForeignKey(d => d.JobPostingId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_job_Post_Activity_jobPostingId");

                entity.HasOne(d => d.Skr)
                    .WithMany(p => p.JobPostActivities)
                    .HasForeignKey(d => d.SkrId)
                    .HasConstraintName("FK_job_Post_Activity_skrId");
            });

            modelBuilder.Entity<JobType>(entity =>
            {
                entity.HasKey(e => e.JobTypeId)
                    .HasName("PK_jobTypeId");

                entity.ToTable("job_Type");

                entity.Property(e => e.JobTypeId)
                    .ValueGeneratedNever()
                    .HasColumnName("jobTypeId");

                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("id");

                entity.Property(e => e.jobTypeDesc)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("jobType");
            });

            modelBuilder.Entity<LocationMaster>(entity =>
            {
                entity.HasKey(e => e.LocationId)
                    .HasName("PK_location_Master_locId");

                entity.ToTable("location_Master");

                entity.Property(e => e.LocationId).HasColumnName("locationId");

                entity.Property(e => e.City)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("city");

                entity.Property(e => e.Country)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("country");

                entity.Property(e => e.State)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("state");
            });

            modelBuilder.Entity<SeekerProfile>(entity =>
            {
                entity.HasKey(e => e.SkrId)
                    .HasName("PK_seeker_Profile_skrId");

                entity.ToTable("seeker_Profile");

                entity.HasIndex(e => e.Aadhaar, "UK_seeker_Profile_aadhaar")
                    .IsUnique();

                entity.HasIndex(e => e.ContactNum, "UK_seeker_Profile_contactNum")
                    .IsUnique();

                entity.HasIndex(e => e.Email, "UK_seeker_Profile_email")
                    .IsUnique();

                entity.HasIndex(e => e.SkrCode, "UK_seeker_Profile_skrCode")
                    .IsUnique();

                entity.Property(e => e.SkrId)
                    .HasColumnName("skrId")
                    .HasDefaultValueSql("(format(getdate(),'yyMMdd')+right('000'+CONVERT([nvarchar](4),NEXT VALUE FOR [dbo].[sequence_Seeker]),(4)))");

                entity.Property(e => e.Aadhaar)
                    .HasColumnType("numeric(12, 0)")
                    .HasColumnName("aadhaar");

                entity.Property(e => e.AltContactNum)
                    .HasColumnType("numeric(15, 0)")
                    .HasColumnName("altContactNum");

                entity.Property(e => e.CommAdd)
                    .IsRequired()
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .HasColumnName("commAdd");

                entity.Property(e => e.ContactNum)
                    .HasColumnType("numeric(15, 0)")
                    .HasColumnName("contactNum");

                entity.Property(e => e.Currency)
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("currency")
                    .IsFixedLength(true);

                entity.Property(e => e.CvHeadLine)
                    .HasMaxLength(2000)
                    .HasColumnName("cvHeadLine");

                entity.Property(e => e.Dob)
                    .HasColumnType("datetime")
                    .HasColumnName("dob");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("email");

                entity.Property(e => e.FbId)
                    .HasMaxLength(200)
                    .HasColumnName("fbId");

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("firstName");

                entity.Property(e => e.Gender)
                    .IsRequired()
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("gender")
                    .IsFixedLength(true);

                entity.Property(e => e.ICatalystRegDate)
                    .HasColumnType("datetime")
                    .HasColumnName("iCatalystRegDate")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.IValueRegDate)
                    .HasColumnType("datetime")
                    .HasColumnName("iValueRegDate")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Income).HasColumnName("income");

                entity.Property(e => e.InstaId)
                    .HasMaxLength(200)
                    .HasColumnName("instaId");

                entity.Property(e => e.IsYearly).HasColumnName("isYearly");

                entity.Property(e => e.LastModifiedDate)
                    .HasColumnType("datetime")
                    .HasColumnName("lastModifiedDate")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("lastName");

                entity.Property(e => e.LkdnId)
                    .HasMaxLength(200)
                    .HasColumnName("lkdnId");

                entity.Property(e => e.MiddleName)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("middleName");

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(200)
                    .HasColumnName("password");

                entity.Property(e => e.PermAdd)
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .HasColumnName("permAdd");

                entity.Property(e => e.ResumeCv)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("resume_Cv");

                entity.Property(e => e.SeekerImage)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("seekerImage");

                entity.Property(e => e.SkrCode)
                    .IsRequired()
                    .HasMaxLength(12)
                    .IsUnicode(false)
                    .HasColumnName("skrCode")
                    .HasDefaultValueSql("(('S-'+format(getdate(),'yyMMdd'))+right('000'+CONVERT([nvarchar](4),NEXT VALUE FOR [dbo].[sequence_Seeker]),(4)))");

                entity.Property(e => e.SkrProfileCreateDate)
                    .HasColumnType("datetime")
                    .HasColumnName("skrProfileCreateDate")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.SkrProfileVisibility).HasColumnName("skrProfileVisibility");

                entity.Property(e => e.SkrTypeId).HasColumnName("skrTypeId");

                entity.Property(e => e.SpokenLanguage)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("spokenLanguage");

                entity.Property(e => e.TwtrId)
                    .HasMaxLength(200)
                    .HasColumnName("twtrId");

                entity.HasOne(d => d.JobLocationPrefNavigation)
                    .WithMany(p => p.SeekerProfiles)
                    .HasForeignKey(d => d.JobLocationPref)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_seeker_Profile_jobLocPref");

                entity.HasOne(d => d.SkrType)
                    .WithMany(p => p.SeekerProfiles)
                    .HasForeignKey(d => d.SkrTypeId)
                    .HasConstraintName("FK_seeker_Profile_skrTypeId");
            });

            modelBuilder.Entity<SeekerType>(entity =>
            {
                entity.HasKey(e => e.SkrTypeId)
                    .HasName("PK_seeker_Type_skrTypeId");

                entity.ToTable("seeker_Type");

                entity.Property(e => e.SkrTypeId)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("skrTypeId");

                entity.Property(e => e.SkrTypeDesc)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("skrTypeDesc");
            });

            modelBuilder.Entity<SeekersSkillsSet>(entity =>
            {
                entity.ToTable("seekers_SkillsSet");

                entity.HasIndex(e => new { e.SkrId, e.SkrCode, e.SkillSetId }, "UK_seekers_SkillsSet_IdCodeSkillSet")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.SkillLevel).HasColumnName("skillLevel");

                entity.Property(e => e.SkillSetId).HasColumnName("skillSetId");

                entity.Property(e => e.SkrCode)
                    .IsRequired()
                    .HasMaxLength(12)
                    .IsUnicode(false)
                    .HasColumnName("skrCode");

                entity.Property(e => e.SkrId).HasColumnName("skrId");

                entity.HasOne(d => d.SkillSet)
                    .WithMany(p => p.SeekersSkillsSets)
                    .HasForeignKey(d => d.SkillSetId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_seekers_SkillsSet_skillSetId");

                entity.HasOne(d => d.SkrCodeNavigation)
                    .WithMany(p => p.SeekersSkillsSetSkrCodeNavigations)
                    .HasPrincipalKey(p => p.SkrCode)
                    .HasForeignKey(d => d.SkrCode)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_seekers_SkillsSet_skrCode");

                entity.HasOne(d => d.Skr)
                    .WithMany(p => p.SeekersSkillsSetSkrs)
                    .HasForeignKey(d => d.SkrId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_seekers_SkillsSet_skrId");
            });

            modelBuilder.Entity<SkillsSet>(entity =>
            {
                entity.HasKey(e => e.SkillSetId)
                    .HasName("PK_skills_Set_SkillSetId");

                entity.ToTable("skills_Set");

                entity.Property(e => e.SkillSetId).HasColumnName("skillSetId");

                entity.Property(e => e.SkillSetName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("skillSetName");

                entity.Property(e => e.SubSkilllSetName)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("subSkilllSetName");
            });

            modelBuilder.HasSequence<int>("sequence_Employer")
                .HasMin(1)
                .HasMax(9999);

            modelBuilder.HasSequence<int>("sequence_jobPost")
                .HasMin(1)
                .HasMax(9999);

            modelBuilder.HasSequence<int>("Sequence_Seeker")
                .HasMin(1)
                .HasMax(9999);

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
