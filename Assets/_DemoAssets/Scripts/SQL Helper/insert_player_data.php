<?php
    error_reporting(E_ALL);
    ini_set('display_errors',1);


    $con = mysqli_connect("localhost","root","awsasnet","unitytestdb");

    // Check connection
    if (mysqli_connect_errno())
    {
        echo "Failed to connect to MySQL: " . mysqli_connect_error();
    }
    
    $new_fb_id = $con->real_escape_string(isset($_GET['new_fb_id']) ? $_GET['new_fb_id'] : '');
    $new_fb_name = $con->real_escape_string(isset($_GET['new_fb_name']) ? $_GET['new_fb_name'] : '');
    $new_fb_friends = $con->real_escape_string(isset($_GET['new_fb_friends']) ? $_GET['new_fb_friends'] : '');
    $new_score = $con->real_escape_string(isset($_GET['new_score']) ? $_GET['new_score'] : 0);
    $new_jump_data = $con->real_escape_string(isset($_GET['new_jump_data']) ? $_GET['new_jump_data'] : '');
    $new_bonus_data = $con->real_escape_string(isset($_GET['new_bonus_data']) ? $_GET['new_bonus_data'] : '');

    if ( !empty($new_fb_id) && !empty($new_fb_name) && !empty($new_fb_friends) && !empty($new_score) && !empty($new_jump_data)  && !empty($new_bonus_data) )
    {
        // Perform queries 
        $query = "INSERT INTO `tbl_player_data` (`fb_id`,`fb_name`,`fb_friends`,`score`,`jump_data`,`bonus_data`) VALUES ('$new_fb_id','$new_fb_name','$new_fb_friends','$new_score','$new_jump_data','$new_bonus_data') ON DUPLICATE KEY UPDATE `fb_name`='$new_fb_name',`fb_friends`='$new_fb_friends',`score`='$new_score',`jump_data`='$new_jump_data',`bonus_data`='$new_bonus_data'";
        mysqli_query($con,$query);
    }

    mysqli_close($con);
?>
