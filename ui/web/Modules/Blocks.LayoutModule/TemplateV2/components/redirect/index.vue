<script>
export default {
  beforeCreate() {
    const { params, query } = this.$route;
    const { path } = params;
    this.$router.replace({
      path: path && path.startsWith("/") ? path : "/" + path,
      query
    });
  },
  abstract:true
  
};
</script>