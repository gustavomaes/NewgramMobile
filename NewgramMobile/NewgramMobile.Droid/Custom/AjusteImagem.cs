using NewgramMobile.Custom;
using Android.Graphics;
using System.IO;

namespace NewgramMobile.Droid.Custom
{
    public class AjusteImagem : IAjusteImagem
    {
        public byte[] Cortar(byte[] imagem, int Largura, int Altura)
        {
            byte[] ImagemRetorno = null;
            byte[] ImagemRedimencioada = null;

            var options = new BitmapFactory.Options();

            int DiferencaLargura = 0;
            int DiferencaAltura = 0;

            //Redimenciona
            using (var ImagemBase = BitmapFactory.DecodeByteArray(imagem, 0, imagem.Length, options))
            {
                if (ImagemBase != null)
                {
                    int LarguraOriginalImagem = ((int)ImagemBase.GetBitmapInfo().Width);
                    int AlturaOriginalImagem = ((int)ImagemBase.GetBitmapInfo().Height);

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

                    using (var bitmapScaled = Bitmap.CreateScaledBitmap(ImagemBase, larguraNova, alturaNova, true))
                    {
                        using (MemoryStream M = new MemoryStream())
                        {
                            bitmapScaled.Compress(Bitmap.CompressFormat.Jpeg, 95, M);

                            //Retorno
                            ImagemRedimencioada = M.ToArray();

                            bitmapScaled.Recycle();
                        }
                    }

                    ImagemBase.Recycle();
                }
            }


            //Corta
            using (var ImagemRedimencioadaBitmap = BitmapFactory.DecodeByteArray(ImagemRedimencioada, 0, ImagemRedimencioada.Length, options))
            {
                //Descobra o X e Y para o corte // Precisa ser exatamento o centro.
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


                using (var ImagemCortada = Bitmap.CreateBitmap(ImagemRedimencioadaBitmap, X, Y, Largura, Altura))
                {
                    using (MemoryStream M = new MemoryStream())
                    {
                        ImagemCortada.Compress(Bitmap.CompressFormat.Jpeg, 100, M);

                        //Retorno
                        ImagemRetorno = M.ToArray();

                        ImagemCortada.Recycle();
                    }
                }
            }

            return ImagemRetorno;
        }
    }
}