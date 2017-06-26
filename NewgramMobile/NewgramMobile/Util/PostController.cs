using NewgramMobile.Models;
using Prism.Commands;
using Prism.Mvvm;
using System;

namespace NewgramMobile.Util
{
    public class PostController : BindableBase
    {
        public DelegateCommand<Post> CurtirCommand { get; }

        public PostController()
        {
            CurtirCommand = new DelegateCommand<Post>(ExecuteCurtirCommand);
        }

        async void ExecuteCurtirCommand(Post post)
        {
            if (post.EuCurti)
            {
                try
                {
                    //API
                    using (APIHelper API = new APIHelper())
                    {
                        await API.PUT("api/posts/curtir/" + post.Id + "/?Curtir=false", null);
                    }

                    post.EuCurti = false;
                    post.QuantidadeCurtidas--;
                }
                catch (HTTPException EX) { }
                catch (Exception EX) { }
            }
            else
            {
                try
                {
                    post.EuCurti = true;
                    post.QuantidadeCurtidas++;

                    //API
                    using (APIHelper API = new APIHelper())
                    {
                        await API.PUT("api/posts/curtir/" + post.Id + "/?Curtir=true", null);
                    }
                }
                catch (HTTPException EX) { }
                catch (Exception EX) { }
            }
        }
    }
}
