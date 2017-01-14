# grekd
Full-featured cloud-based grekan-service

#### Api functions:

`create_random_grekan`

takes: nothing<br>
returns: image/jpeg

`get_post`

takes: id (int) - query param<br>
returns: json

`add_image`

takes: key (string) - header, image (raw bytes) request body<br>
returns: string

`delete_image`

takes: key (string) - header, id (int) - query param<br>
returns: string

`get_post_in_range`

takes: from (Date) - query param, to (Date) - query param<br>
returns: json

`count_posts`

takes: nothing<br>
returns: int

`count_images`

takes: nothing<br>
returns: int
