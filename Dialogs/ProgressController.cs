﻿using System;
using System.Threading.Tasks;
using WinDurango.UI.Utils;

namespace WinDurango.UI.Dialogs
{
    public class ProgressController
    {
        private readonly ProgressDialog _dialog;
        private bool hasFailed = false;

        public ProgressController(ProgressDialog dialog)
        {
            _dialog = dialog;
        }

        public void UpdateProgress(double progress)
        {
            _dialog.DispatcherQueue.TryEnqueue(() =>
            {
                _dialog.Progress = progress;
            });
        }

        public void UpdateText(string text)
        {
            _dialog.DispatcherQueue.TryEnqueue(() =>
            {
                _dialog.Text = text;
            });
        }

        public void Update(string text, double progress)
        {
            _dialog.DispatcherQueue.TryEnqueue(() =>
            {
                _dialog.Text = text;
                _dialog.Progress = progress;
            });
        }

        public async Task Fail(string title, string reason)
        {
            hasFailed = true;
            Logger.WriteError($"[{this._dialog.PTitle}] {title}: {reason}");
            _dialog.DispatcherQueue.TryEnqueue(async () =>
            {
                _dialog.Hide();
                NoticeDialog oops = new NoticeDialog(title, reason);
                await oops.Show();
            });
        }

        public async Task Fail(string title, Exception ex)
        {
            hasFailed = true;
            Logger.WriteException(ex);
            _dialog.DispatcherQueue.TryEnqueue(async () =>
            {
                _dialog.Hide();
                NoticeDialog oops = new NoticeDialog(title, ex.Message);
                await oops.Show();
            });
        }

        public bool failed
        {
            get => hasFailed;
        }

        public void Close()
        {
            _dialog.DispatcherQueue.TryEnqueue(() =>
            {
                _dialog.Hide();
            });
        }

        public void Show()
        {
            _dialog.DispatcherQueue.TryEnqueue(() =>
            {
                _dialog.ShowAsync();
            });
        }

        public async Task Create(Action action)
        {
            _dialog.ShowAsync();
            try
            {
                action();
                _dialog.Hide();
            }
            catch (Exception ex)
            {
                _dialog.Hide();
                NoticeDialog oops = new NoticeDialog(ex.Message, "Error");
                await oops.Show();
            }
        }

        public async Task CreateAsync(Func<Task> action)
        {
            _dialog.ShowAsync();
            try
            {
                await action();
                _dialog.Hide();
            }
            catch (Exception ex)
            {
                _dialog.Hide();
                NoticeDialog oops = new NoticeDialog(ex.Message, "Error");
                await oops.Show();
            }
        }
    }
}