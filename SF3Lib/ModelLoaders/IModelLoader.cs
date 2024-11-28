using System;
using SF3.Models.Files;
using SF3.RawData;

namespace SF3.ModelLoaders {
    /// <summary>
    /// Interface for loading models.
    /// </summary>
    public interface IModelLoader : IModifiable, IDisposable {
        /// <summary>
        /// Closes a model if opened. Invokes events 'PreClosed' and 'Closed' if an model was closed.
        /// </summary>
        /// <returns>'true' if 'IsLoaded' is 'false' after closing is completed or aborted.  Otherwise 'false'.</returns>
        bool Close();

        /// <summary>
        /// The title used for the model, considering whether or not one is loaded.
        /// </summary>
        string ModelTitle(string formTitle);

        /// <summary>
        /// The model loaded and created, backed by an underlying IRawData.
        /// </summary>
        IBaseFile Model { get; }

        /// <summary>
        /// The raw data that exists when IsLoaded is 'true'.
        /// </summary>
        IRawData RawData { get; }

        /// <summary>
        /// 'True' when the model is loaded.
        /// </summary>
        bool IsLoaded { get; }

        /// <summary>
        /// The title of the loaded data being modified for display.
        /// </summary>
        string Title { get; }

        /// <summary>
        /// Event that occurs when the 'IsLoaded' property is changed.
        /// </summary>
        event EventHandler IsLoadedChanged;

        /// <summary>
        /// Event that occurs before the stream is opened.
        /// </summary>
        event EventHandler PreLoaded;

        /// <summary>
        /// Event that occurs when the stream is opened.
        /// </summary>
        event EventHandler Loaded;

        /// <summary>
        /// Event that occurs before the stream is saved.
        /// </summary>
        event EventHandler PreSaved;

        /// <summary>
        /// Event that occurs when the stream is saved.
        /// </summary>
        event EventHandler Saved;

        /// <summary>
        /// Event that occurs before the stream is closed.
        /// </summary>
        event EventHandler PreClosed;

        /// <summary>
        /// Event that occurs when the stream is closed.
        /// </summary>
        event EventHandler Closed;

        /// <summary>
        /// Event that occurs whenever the 'Title' property is changed.
        /// </summary>
        event EventHandler TitleChanged;
    }
}
