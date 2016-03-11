using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace RedisExplorer.Controls
{
    /// <summary>
    /// Class used to have an image that is able to be gray when the control is not enabled.
    /// Based on the version by Thomas LEBRUN (http://blogs.developpeur.org/tom)
    /// </summary>
    /// <seealso cref="http://stackoverflow.com/questions/11305577/grey-out-image-on-button-when-button-is-disabled-simple-and-beautiful-way"/>
    public class GreyableImage : Image
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GreyableImage"/> class.
        /// </summary>
        static GreyableImage()
        {
            // Override the metadata of the IsEnabled and Source property.
            IsEnabledProperty.OverrideMetadata(typeof(GreyableImage), new FrameworkPropertyMetadata(true, OnAutoGreyScaleImageIsEnabledPropertyChanged));
            SourceProperty.OverrideMetadata(typeof(GreyableImage), new FrameworkPropertyMetadata(null, OnAutoGreyScaleImageSourcePropertyChanged));
        }

        protected static GreyableImage GetImageWithSource(DependencyObject source)
        {
            var image = source as GreyableImage;

            return image?.Source == null ? null : image;
        }

        /// <summary>
        /// Called when [auto grey scale image source property changed].
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="args">The <see cref="System.Windows.DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        protected static void OnAutoGreyScaleImageSourcePropertyChanged(DependencyObject source, DependencyPropertyChangedEventArgs args)
        {
            var image = GetImageWithSource(source);
            if (image != null)
            {
                ApplyGreyScaleImage(image, image.IsEnabled);
            }
        }

        /// <summary>
        /// Called when [auto grey scale image is enabled property changed].
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="args">The <see cref="System.Windows.DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        protected static void OnAutoGreyScaleImageIsEnabledPropertyChanged(DependencyObject source, DependencyPropertyChangedEventArgs args)
        {
            var image = GetImageWithSource(source);
            if (image == null) { return; }
            var isEnabled = Convert.ToBoolean(args.NewValue);
            ApplyGreyScaleImage(image, isEnabled);
        }

        protected static void ApplyGreyScaleImage(GreyableImage greyScaleImg, Boolean isEnabled)
        {
            try
            {
                if (!isEnabled)
                {
                    BitmapSource bitmapImage;

                    if (greyScaleImg.Source is FormatConvertedBitmap)
                    {
                        // Already grey !
                        return;
                    }
                    if (greyScaleImg.Source is BitmapSource)
                    {
                        bitmapImage = (BitmapSource)greyScaleImg.Source;
                    }
                    else // trying string 
                    {
                        bitmapImage = new BitmapImage(new Uri(greyScaleImg.Source.ToString()));
                    }
                    var conv = new FormatConvertedBitmap(bitmapImage, PixelFormats.Gray32Float, null, 0);
                    greyScaleImg.Source = conv;

                    // Create Opacity Mask for greyscale image as FormatConvertedBitmap does not keep transparency info
                    greyScaleImg.OpacityMask = new ImageBrush(((FormatConvertedBitmap)greyScaleImg.Source).Source); //equivalent to new ImageBrush(bitmapImage)
                }
                else
                {
                    if (greyScaleImg.Source is FormatConvertedBitmap)
                    {
                        greyScaleImg.Source = ((FormatConvertedBitmap)greyScaleImg.Source).Source;
                    }
                    else if (greyScaleImg.Source is BitmapSource)
                    {
                        // Should be full color already.
                        return;
                    }

                    // Reset the Opcity Mask
                    greyScaleImg.OpacityMask = null;
                }
            }
            catch (Exception)
            {
                // nothin'
            }

        }

    }
}