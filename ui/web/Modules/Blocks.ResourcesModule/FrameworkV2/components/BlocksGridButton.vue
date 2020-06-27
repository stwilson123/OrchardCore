<script>
import BlocksButton from "./BlocksButton.vue";
export default {
  name: "BlocksGridButton",
  data() {
    return {
      params: {}
    };
  },
  mixins: [BlocksButton],
  methods: {},
  created() {
    this.size = this.params.size;
    this.type = this.params.type;
    this.plain = this.params.plain;
    this.round = this.params.round;
    this.circle = this.params.circle;
    this.loading = this.params.loading;
    this.disabled = this.params.disabled;
    this.icon = this.params.icon;
  }
};
</script>