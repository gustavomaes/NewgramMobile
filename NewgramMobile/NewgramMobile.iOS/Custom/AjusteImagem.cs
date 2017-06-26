using Foundation;
using UIKit;
using NewgramMobile.Custom;
using System.Drawing;

namespace NewgramMobile.iOS.Custom
{
    public class AjusteImagem : IAjusteImagem
    {
        public byte[] Cortar(byte[] imagem, int Largura, int Altura)
        {
            using (var dadosImagem = NSData.FromArray(imagem))
            {
                using (var imagemParaCortar = UIImage.LoadFromData(dadosImagem))
                {
                    int DiferencaLargura = 0;
                    int DiferencaAltura = 0;

                    //REDIMENCIONA PROPOSCIONALMENTE
                    int LarguraOriginalImagem = ((int)imagemParaCortar.Size.Width);
                    int AlturaOriginalImagem = ((int)imagemParaCortar.Size.Height);

                    var larguraNova = Largura;
                    var alturaNova = Altura;

                    if (LarguraOriginalImagem > AlturaOriginalImagem)
                    {
                        larguraNova = ((int)((double)Largura * ((double)LarguraOriginalImagem / (double)AlturaOriginalImagem)));
                        DiferencaLargura = larguraNova - Largura;
                    }
                    else if (AlturaOriginalImagem > LarguraOriginalImagem)
                    {
                        alturaNova = ((int)((double)Altura * ((double)AlturaOriginalImagem / (double)LarguraOriginalImagem)));
                        DiferencaAltura = alturaNova - Altura;
                    }

                    UIGraphics.BeginImageContext(new SizeF(larguraNova, alturaNova));

                    imagemParaCortar.Draw(new RectangleF(0, 0, larguraNova, alturaNova));

                    var imagemRedimencionada = UIGraphics.GetImageFromCurrentImageContext();

                    UIGraphics.EndImageContext();


                    //CORTA

                    int X = 0;
                    int Y = 0;

                    if (DiferencaLargura > 0)
                    {
                        X = DiferencaLargura / 2;
                    }

                    if (DiferencaAltura > 0)
                    {
                        Y = DiferencaAltura / 2;
                    }

                    var imgSize = imagemRedimencionada.Size;

                    UIGraphics.BeginImageContext(new SizeF(Largura, Altura));

                    var context = UIGraphics.GetCurrentContext();
                    var clippedRect = new RectangleF(0, 0, Largura, Altura);
                    context.ClipToRect(clippedRect);
                    var drawRect = new RectangleF(-X, -Y, (float)imgSize.Width, (float)imgSize.Height);
                    imagemRedimencionada.Draw(drawRect);
                    var modifiedImage = UIGraphics.GetImageFromCurrentImageContext();

                    UIGraphics.EndImageContext();

                    return modifiedImage.AsJPEG().ToArray();
                }
            }
        }
    }
}