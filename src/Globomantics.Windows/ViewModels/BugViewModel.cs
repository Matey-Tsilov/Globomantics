﻿using CommunityToolkit.Mvvm.Input;
using Globomantics.Domain;
using Globomantics.Infrastructure.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Globomantics.Windows.ViewModels;

public class BugViewModel : BaseToDoViewModel<Bug>
{
    private readonly IRepository<Bug> repository;

    private string? description;
    private string? affectedVersion;
    private int affectedUsers;
    private DateTimeOffset dueDate;
    private Severity severity;

    public string? Description
    {
        get => description;
        set
        {
            description = value;
            OnPropertyChanged(nameof(Description));
        }
    }
    public string AffectedVersion
    {
        get => affectedVersion;
        set
        {
            affectedVersion = value;
            OnPropertyChanged(nameof(AffectedVersion));
        }
    }
    public DateTimeOffset DueDate
    {
        get => dueDate;
        set
        {
            dueDate = value;
            OnPropertyChanged(nameof(DueDate));
        }
    }
    public int AffectedUsers
    {
        get => affectedUsers;
        set
        {
            affectedUsers = value;
            OnPropertyChanged(nameof(AffectedUsers));
        }
    }
    public Severity Severity
    {
        get => severity;
        set
        {
            severity = value;
            OnPropertyChanged(nameof(Severity));
        }
    }

    public IEnumerable<Severity> SeverityLevels { get; } = new[]
    {
        Severity.Critical,
        Severity.Major,
        Severity.Minor,
        Severity.Annoying,

    };
    public ObservableCollection<byte[]> Screenshots { get; set; } = new();   
    public ICommand AttachScreenshotCommand { get; set; }
    public BugViewModel(IRepository<Bug> repository) : base()
    {
        this.repository = repository;
        SaveCommand = new RelayCommand(async () => await SaveAsync());

        AttachScreenshotCommand = new RelayCommand(() =>
        {
            var filenames = ShowOpenFileDialog?.Invoke();

            if (filenames is null || !filenames.Any()) 
            {
                return;
            }

            foreach (var filename in filenames) 
            {
                Screenshots.Add(File.ReadAllBytes(filename));
            }
        });
    }
    public override void UpdateModel(Todo model)
    {
        if (model is not Bug bug) return;

        base.UpdateModel(bug);

    Description = bug.Description;
    AffectedVersion = bug.AffectedVersion;
    AffectedUsers = bug.AffectedUsers;
    DueDate = bug.DueDate;
    Screenshots = new(bug.Images);
    Severity = bug.Severity;
}
    public override async Task SaveAsync()
    {
        if (string.IsNullOrWhiteSpace(Title))
        {
            ShowError?.Invoke($"{nameof(Title)} cannot be empty!");
            return;
        }
        if (Model is null)
        {
            Model = new Bug(
                Title,
                Description ?? "No Description",
                Severity,
                AffectedVersion,
                AffectedUsers,
                App.CurrentUser,
                App.CurrentUser,
                Screenshots.ToArray())
            {
                DueDate = DueDate,
                Parent = Parent,
                IsCompleted = IsCompleted
            };

        }
        else
        {
            Model = Model with
            {
                Title = Title,
                Description = Description ?? "No Description",
                Severity = Severity,
                AffectedVersion = AffectedVersion,
                AffectedUsers = AffectedUsers,
                DueDate = DueDate,
                Parent = Parent,
                IsCompleted = IsCompleted,
                Images = Screenshots.ToArray()
            };

        }

        await repository.AddAsync(Model);
        await repository.SaveChangesAsync();
    }
}
