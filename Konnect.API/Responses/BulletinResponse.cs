﻿using UTCClassSupport.API.Responses.DTOs;

namespace UTCClassSupport.API.Responses
{
  public class AddPostCommentResponse : Response
  {
    public CommentDTO Comment { get; set; }
  }


  public class ChangePostStateResponse : Response
  {
  }
  public class GetPostResponse : Response
  {
    public IEnumerable<PostDTO> Posts { get; set; }
  }
  public class UploadNewsToBulletinResponse : Response
  {
  }
}